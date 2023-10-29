using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IOManager.Database;
using IOManager.Models;
using IOManager.Utils;
using IOManager.Views;

namespace IOManager.ViewModels
{
	public partial class OrderCreateEditViewModel : ObservableObject, IQueryAttributable
	{
		public OrderCreateEditViewModel(DbConnection connection)
		{
			Connection = connection;
			Lines = new ObservableCollection<OrderLineModel>();
		}

		public ObservableCollection<OrderLineModel> Lines { get; }

		[ObservableProperty]
		string customerName;

		[ObservableProperty]
		bool isWholeSale = true;

		[RelayCommand]
		async Task AddItems()
		{
			var selectItemParameterDict = new Dictionary<string, object>()
			{
				{ GlobalConstants.ItemSelect, true },
			};
			await Shell.Current.GoToAsync($"{nameof(ItemsSearchPage)}", selectItemParameterDict);
		}

		[RelayCommand]
		void IsWholeSaleTapped()
		{
			IsWholeSale = !IsWholeSale;
		}

		[RelayCommand]
		async Task Back()
		{
			await Shell.Current.GoToAsync("..");
		}

		[RelayCommand]
		void Clear()
		{
			CustomerName = null;
			IsWholeSale = true;
			Lines.Clear();
		}

		[RelayCommand]
		async Task Save()
		{
			if (!await IsValid())
			{
				return;
			}

			var header = new OrderHeaderModel
			{
				CustomerName = CustomerName,
				OrderNo = "Auto",
			};

			await Connection.Create(header);

			foreach (var line in Lines)
			{
				line.OrderId = header.Id;
			}

			await Connection.Create(Lines);

			Clear();
		}

		[RelayCommand]
		void QtyDec(object obj)
		{
			if (obj is OrderLineModel line && --line.Qty == default)
			{
				Lines.Remove(line);
			}
		}

		[RelayCommand]
		void QtyInc(object obj)
		{
			if (obj is OrderLineModel line)
			{
				line.Qty++;
			}
		}

		async Task<bool> IsValid()
		{
			if (Lines.Count == 0)
			{
				await Shell.Current.DisplayAlert("Error!", "Add items first", "Ok");
				return false;
			}

			if (Lines.Any(x => x.Price == default))
			{
				await Shell.Current.DisplayAlert("Error!", "Enter price for all items", "Ok");
				return false;
			}

			if (Lines.Any(x => x.Qty == default))
			{
				await Shell.Current.DisplayAlert("Error!", "Enter Qty for all items", "Ok");
				return false;
			}

			return true;
		}

		void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
		{
			if (query.TryGetValue(GlobalConstants.SelectedItems, out var value) && value is IEnumerable<ItemModel> selectedItems)
			{
				foreach (var item in selectedItems)
				{
					Lines.Add(new OrderLineModel()
					{
						ItemName = item.ItemName,
						Price = IsWholeSale ? item.WholeSalePrice : item.RetailSalePrice ?? 0m,
						Qty = 1
					});
				}
			}
		}

		DbConnection Connection { get; }
	}
}
