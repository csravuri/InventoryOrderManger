﻿using System.Collections.ObjectModel;
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
			Lines = new ObservableCollection<OrderLineViewModel>();
		}

		public ObservableCollection<OrderLineViewModel> Lines { get; }

		[ObservableProperty]
		string title = OrderCreateCaption;

		[ObservableProperty]
		string customerName;

		[ObservableProperty]
		bool isWholeSale = true;

		[ObservableProperty]
		bool isNewEntryEveryItem = false;

		partial void OnIsWholeSaleChanged(bool value)
		{
			foreach (var line in Lines)
			{
				line.Price = IsWholeSale ? line.Item.WholeSalePrice : line.Item.RetailSalePrice;
			}
		}

		[ObservableProperty]
		[NotifyPropertyChangedFor(nameof(IsTotalVisible))]
		decimal total;

		public bool IsTotalVisible => Total > 0m;

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
		void IsNewEntryEveryItemTapped()
		{
			IsNewEntryEveryItem = !IsNewEntryEveryItem;
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
			IsNewEntryEveryItem = false;
			Total = 0m;
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
				Total = Total,
				LinesCount = Lines.Count
			};

			await Connection.Create(header);

			var lines = Lines.Select(x => new OrderLineModel
			{
				ItemName = x.ItemName,
				Qty = x.Qty,
				Price = x.Price,
				OrderId = header.Id
			});

			await Connection.Create(lines);

			Clear();
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
			if (query.TryGetValue(GlobalConstants.SelectedItems, out var value) && value is IEnumerable<ItemViewModel> selectedItems)
			{
				foreach (var itemViewModel in selectedItems)
				{

					var newItem = itemViewModel.Item;
					var newItemPrice = IsWholeSale ? newItem.WholeSalePrice : newItem.RetailSalePrice;
					var existingItem = Lines.FirstOrDefault(x => x.Item.Id == newItem.Id && x.Price == newItemPrice);
					if (!IsNewEntryEveryItem && existingItem != null)
					{
						existingItem.Qty++;
					}
					else
					{
						Lines.Add(new OrderLineViewModel(OnLineQtyChanged, newItem)
						{
							ItemName = newItem.ItemName,
							Price = newItemPrice,
							Qty = 1
						});
					}
				}

				UpdateTotal();
			}
		}

		void OnLineQtyChanged(OrderLineViewModel line)
		{
			if (line.Qty == default)
			{
				Lines.Remove(line);
			}

			UpdateTotal();
		}

		void UpdateTotal()
		{
			Total = Lines.Sum(x => x.Price * x.Qty);
		}

		DbConnection Connection { get; }

		const string OrderCreateCaption = "Order Create";
		const string OrderUpdateCaption = "Order Update";
	}
}
