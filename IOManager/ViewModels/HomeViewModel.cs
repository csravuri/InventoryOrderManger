using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IOManager.Views;

namespace IOManager.ViewModels
{
	public partial class HomeViewModel : ObservableObject
	{
		[ObservableProperty]
		string title = "IO Manager";

		[RelayCommand]
		async Task ItemCreate()
		{
			await Shell.Current.GoToAsync($"{nameof(ItemCreateEditPage)}");
		}

		[RelayCommand]
		async Task ItemView()
		{
			await Shell.Current.GoToAsync($"{nameof(ItemsSearchPage)}");
		}

		[RelayCommand]
		async Task OrderCreate()
		{
			await Shell.Current.GoToAsync($"{nameof(OrderCreateEditPage)}");
		}

		[RelayCommand]
		async Task OrderView()
		{
			await Shell.Current.GoToAsync($"{nameof(OrdersSearchPage)}");
		}

		[RelayCommand]
		async Task CustomerCreate()
		{
			await Shell.Current.GoToAsync($"{nameof(CustomerCreateEditPage)}");
		}

		[RelayCommand]
		async Task CustomerView()
		{
			await Shell.Current.GoToAsync($"{nameof(CustomerSearchPage)}");
		}

		[RelayCommand]
		async Task Backup()
		{
			await Shell.Current.GoToAsync($"{nameof(BackupPage)}");
		}

		[RelayCommand]
		async Task Restore()
		{
			await Shell.Current.GoToAsync($"{nameof(RestorePage)}");
		}

		[RelayCommand]
		async Task OfflineSync()
		{
			await Shell.Current.GoToAsync($"{nameof(OfflineSyncPage)}");
		}

		[RelayCommand]
		async Task OnlineSync()
		{
			await Shell.Current.GoToAsync($"{nameof(OnlineSyncPage)}");
		}
	}
}
