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
				var resultWhenDefaultImage = await Shell.Current.DisplayActionSheet(ImageCaption, BackCaption, string.Empty, GalleryCaption, CameraCaption);
				await GetImage(resultWhenDefaultImage);
				return;
			}

			var result = await Shell.Current.DisplayActionSheet(ImageCaption, BackCaption, RemoveCaption, GalleryCaption);
			if (result == RemoveCaption)
			{
				ImagePath = DefaultImagePath;
				return;
			}

			await GetImage(result);
		}

		async Task GetImage(string pickerResult)
		{
			if (pickerResult == GalleryCaption)
			{
				await PickImage();
			}
			else if (pickerResult == CameraCaption)
			{
				await CaptureImage();
			}
		}

		async Task CaptureImage()
		{
			var options = new MediaPickerOptions();
			options.Title = "Take Image";
			var captureResult = await MediaPicker.CapturePhotoAsync(options);
			var capturedImagePath = captureResult?.FullPath;
			if (!string.IsNullOrEmpty(capturedImagePath))
			{
				var destiantion = Path.Combine(FileSystem.AppDataDirectory, Path.GetFileName(capturedImagePath));
				File.Move(capturedImagePath, destiantion);
				ImagePath = destiantion;
			}
		}

		async Task PickImage()
		{
			var options = new MediaPickerOptions();
			options.Title = "Pick Image";
			var captureResult = await MediaPicker.PickPhotoAsync(options);
			var pickedImagePath = captureResult?.FullPath;
			if (!string.IsNullOrEmpty(pickedImagePath))
			{
				var destiantion = Path.Combine(FileSystem.AppDataDirectory, Path.GetFileName(pickedImagePath));
				File.Copy(pickedImagePath, destiantion);
				ImagePath = pickedImagePath;
			}
		}

		const string DefaultImagePath = "default_image.png";
		const string BackCaption = "Back";
		const string RemoveCaption = "Remove";
		const string GalleryCaption = "Gallery";
		const string CameraCaption = "Camera";
		const string ImageCaption = "Image";
	}
}
