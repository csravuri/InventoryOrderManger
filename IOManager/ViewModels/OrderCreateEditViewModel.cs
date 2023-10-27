using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IOManager.Database;

namespace IOManager.ViewModels
{
	public partial class OrderCreateEditViewModel : ObservableObject
	{
		public OrderCreateEditViewModel(DbConnection connection)
		{
			Connection = connection;
		}

		[ObservableProperty]
		string customerName;

		[ObservableProperty]
		bool isWholeSale = true;

		[RelayCommand]
		async Task AddItems()
		{
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
		}

		[RelayCommand]
		async Task Save()
		{
		}

		DbConnection Connection { get; }
	}
}
