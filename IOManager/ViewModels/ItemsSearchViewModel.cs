using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IOManager.Database;
using IOManager.Models;
using IOManager.Utils;
using IOManager.Views;

namespace IOManager.ViewModels
{
	public partial class ItemsSearchViewModel : ObservableObject
	{
		public ItemsSearchViewModel(DbConnection connection)
		{
			Connection = connection;
			Items = new ObservableCollection<ItemModel>();
		}

		public ObservableCollection<ItemModel> Items { get; }

		Task<IEnumerable<ItemModel>> AllItems => allItems ??= GetAllItems();
		Task<IEnumerable<ItemModel>> allItems;
		async Task<IEnumerable<ItemModel>> GetAllItems()
		{
			return await Connection.GetAll<ItemModel>(x => true);
		}

		[ObservableProperty]
		string title = "Items Search";


		[ObservableProperty]
		string searchText;

		[RelayCommand]
		async Task Search(bool? reloadItems)
		{
			if (reloadItems == true)
			{
				allItems = null;
			}

			Items.Clear();
			var items = await AllItems;
			foreach (var item in items.Where(IsItemNeeded))
			{
				Items.Add(item);
			}
		}

		bool IsItemNeeded(ItemModel item)
		{
			return string.IsNullOrEmpty(SearchText)
				|| SearchText.Split(" ").Where(x => !string.IsNullOrEmpty(x)).Any(item.ItemName.Contains);
		}

		[RelayCommand]
		async Task AddNew()
		{
			var searchItemParameterDict = new Dictionary<string, object>()
			{
				{ GlobalConstants.ItemSearchText, SearchText },
			};
			await Shell.Current.GoToAsync($"./{nameof(ItemCreateEditPage)}", searchItemParameterDict);
		}

		[RelayCommand]
		async Task Selected(object obj)
		{
			if (obj is ItemModel model)
			{
				var updateItemParameterDict = new Dictionary<string, object>()
				{
					{ GlobalConstants.ItemUpdate, model },
				};
				await Shell.Current.GoToAsync($"./{nameof(ItemCreateEditPage)}", updateItemParameterDict);

			}
		}

		DbConnection Connection { get; }
	}
}
