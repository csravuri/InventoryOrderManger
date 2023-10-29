using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IOManager.Database;
using IOManager.Models;

namespace IOManager.ViewModels
{
	public partial class OrdersSearchViewModel : ObservableObject
	{
		public OrdersSearchViewModel(DbConnection connection)
		{
			Connection = connection;
			Orders = new ObservableCollection<OrderHeaderModel>();
		}

		public ObservableCollection<OrderHeaderModel> Orders { get; }

		[ObservableProperty]
		string title = OrdersSearchCaption;

		[ObservableProperty]
		string customerName;

		[ObservableProperty]
		DateTime fromDate = DateTime.Today;

		[ObservableProperty]
		DateTime toDate = DateTime.Today;

		[RelayCommand]
		async Task Search()
		{
			Orders.Clear();
			var orders = await Connection.GetAll<OrderHeaderModel>(x => true);
			foreach (var order in orders.Where(IsOrderNeeded))
			{
				Orders.Add(order);
			}
		}

		bool IsOrderNeeded(OrderHeaderModel order)
		{
			return //(FromDate is null) && (ToDate is null) && 
				(string.IsNullOrEmpty(CustomerName) || CustomerName.Split(" ").Where(x => !string.IsNullOrEmpty(x)).Any(order.CustomerName.Contains));
		}

		DbConnection Connection { get; }

		const string OrdersSearchCaption = "Orders Search";
	}
}
