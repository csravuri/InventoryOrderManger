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

		[ObservableProperty]
		string title = "Items Search";


		[ObservableProperty]
		string searchText;

		[RelayCommand]
		async Task Search()
		{
			if (string.IsNullOrWhiteSpace(SearchText))
			{
				return;
			}

			Items.Clear();
			var items = await Connection.GetAll<ItemModel>(x => x.ItemName.ToLower().Contains(SearchText.ToLower()));

			foreach (var item in items)
			{
				Items.Add(item);
			}

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

		DbConnection Connection { get; }
	}
}
