using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace IOManager.ViewModels
{
	public partial class ItemCreateEditViewModel : ObservableObject
	{
		[ObservableProperty]
		string itemName;

		[ObservableProperty]
		decimal? wholeSalePrice;

		[ObservableProperty]
		decimal? retailSalePrice;

		[ObservableProperty]
		decimal? purchasePrice;

		[ObservableProperty]
		int? stockQuantity;

		[ObservableProperty]
		string description;

		[ObservableProperty]
		string imagePath = DefaultImagePath;

		[RelayCommand]
		async Task Back()
		{
			await Shell.Current.GoToAsync("..");
		}

		[RelayCommand]
		void Clear()
		{
			ItemName = null;
			WholeSalePrice = null;
			RetailSalePrice = null;
			PurchasePrice = null;
			StockQuantity = null;
			Description = null;
		}

		[RelayCommand]
		async Task Save()
		{
			await Shell.Current.DisplayAlert("Saved", "Not really :)", "OK");
		}

		[RelayCommand]
		async Task SaveAndBack()
		{
			await Save();
			await Back();
		}

		[RelayCommand]
		async Task ImageTap()
		{
			if (ImagePath == DefaultImagePath)
			{
				await CaptureImage();
				return;
			}

			var result = await Shell.Current.DisplayActionSheet("Image", BackCaption, RemoveCaption, UpdateCaption);
			if (result == RemoveCaption)
			{
				ImagePath = DefaultImagePath;
			}
			else if (result == UpdateCaption)
			{
				await CaptureImage();
			}
		}

		async Task CaptureImage()
		{
			await Task.CompletedTask;
		}

		const string DefaultImagePath = "default_image.png";
		const string BackCaption = "Back";
		const string RemoveCaption = "Remove";
		const string UpdateCaption = "Update";
	}
}
