using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IOManager.Views;

namespace IOManager.ViewModels
{
	public partial class HomeViewModel : ObservableObject
	{
		[RelayCommand]
		async Task ItemCreate()
		{
			await Shell.Current.GoToAsync($"{nameof(ItemCreateEditPage)}");
		}

		[RelayCommand]
		void ItemView()
		{
		}

		[RelayCommand]
		void OrderCreate()
		{
		}

		[RelayCommand]
		void OrderView()
		{
		}

		[RelayCommand]
		void Backup()
		{
		}

		[RelayCommand]
		void Restore()
		{
		}
	}
}
