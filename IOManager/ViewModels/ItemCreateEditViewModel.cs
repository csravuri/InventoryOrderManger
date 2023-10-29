using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IOManager.Database;
using IOManager.Models;
using IOManager.Utils;
using SQLite;

namespace IOManager.ViewModels
{
	public partial class ItemCreateEditViewModel : ObservableObject, IQueryAttributable
	{
		public ItemCreateEditViewModel(DbConnection connection)
		{
			Connection = connection;
		}

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

		[ObservableProperty]
		string title = CreateItemCaption;

		[ObservableProperty]
		bool isCreate = true;

		[RelayCommand]
		async Task SaveAndBack()
		{
			if (await Save())
			{
				await Back();
			}
		}

		[RelayCommand]
		async Task<bool> Save()
		{
			if (string.IsNullOrEmpty(ItemName))
			{
				await Shell.Current.DisplayAlert("Error!", "Item Name required", "Ok");
				return false;
			}

			if (WholeSalePrice is null && RetailSalePrice is null)
			{
				await Shell.Current.DisplayAlert("Error!", "Wholesale or Retail price required", "Ok");
				return false;
			}

			try
			{
				var model = new ItemModel
				{
					ItemName = ItemName,
					WholeSalePrice = WholeSalePrice ?? RetailSalePrice ?? 0m,
					RetailSalePrice = RetailSalePrice ?? WholeSalePrice ?? 0m,
					PurchasePrice = PurchasePrice,
					StockQuantity = StockQuantity,
					Description = Description,
					ImagePath = ImagePath
				};

				if (IsCreate)
				{
					await Connection.Create(model);
					Clear();
				}
				else
				{
					model.Id = modelId;
					await Connection.Update(model);
				}

				return true;
			}
			catch (SQLiteException slx) when (slx.Message.Contains("UNIQUE"))
			{
				await Shell.Current.DisplayAlert("Error!", "Item Name already exists", "Ok");
				return false;
			}
			catch (Exception ex)
			{
				await Shell.Current.DisplayAlert("Error!", ex.Message, "Ok");
				return false;
			}
		}

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
			ImagePath = DefaultImagePath;
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

			var result = await Shell.Current.DisplayActionSheet(ImageCaption, BackCaption, RemoveCaption, GalleryCaption, CameraCaption);
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
			var options = new MediaPickerOptions
			{
				Title = "Take Image"
			};
			var captureResult = await MediaPicker.CapturePhotoAsync(options);
			var capturedImagePath = captureResult?.FullPath;
			if (!string.IsNullOrEmpty(capturedImagePath))
			{
				var destiantion = GetDestination(capturedImagePath);
				File.Move(capturedImagePath, destiantion);
				ImagePath = destiantion;
			}
		}

		async Task PickImage()
		{
			var options = new MediaPickerOptions
			{
				Title = "Pick Image"
			};
			var captureResult = await MediaPicker.PickPhotoAsync(options);
			var pickedImagePath = captureResult?.FullPath;
			if (!string.IsNullOrEmpty(pickedImagePath))
			{
				var destiantion = GetDestination(pickedImagePath, true);
				File.Copy(pickedImagePath, destiantion);
				ImagePath = destiantion;
			}
		}

		string GetDestination(string sourcePath, bool newFileForDestiantion = false)
		{
			if (!Directory.Exists(ImagesSubFolderPath))
			{
				Directory.CreateDirectory(ImagesSubFolderPath);
			}

			var fileName = Path.GetFileNameWithoutExtension(sourcePath);
			var extention = Path.GetExtension(sourcePath);
			if (newFileForDestiantion)
			{
				fileName = Guid.NewGuid().ToString().Replace("-", "");
			}

			return Path.Combine(ImagesSubFolderPath, fileName + extention);
		}

		void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
		{
			if (query.TryGetValue(GlobalConstants.ItemSearchText, out var searchTextValue) && searchTextValue is string itemSearchText)
			{
				ItemName = itemSearchText;
			}
			else if (query.TryGetValue(GlobalConstants.ItemUpdate, out var modelValue) && modelValue is ItemModel model)
			{
				ItemName = model.ItemName;
				WholeSalePrice = model.WholeSalePrice;
				RetailSalePrice = model.RetailSalePrice;
				PurchasePrice = model.PurchasePrice;
				StockQuantity = model.StockQuantity;
				Description = model.Description;
				ImagePath = model.ImagePath;
				modelId = model.Id;

				Title = UpdateItemCaption;
				IsCreate = false;
			}
		}

		const string DefaultImagePath = "default_image.png";
		const string BackCaption = "Back";
		const string RemoveCaption = "Remove";
		const string GalleryCaption = "Gallery";
		const string CameraCaption = "Camera";
		const string ImageCaption = "Image";
		const string ImagesSubFolder = "Images";
		const string CreateItemCaption = "Create Item";
		const string UpdateItemCaption = "Update Item";

		DbConnection Connection { get; }
		readonly string ImagesSubFolderPath = Path.Combine(GlobalConstants.RootFolder, ImagesSubFolder);
		Guid modelId;
	}
}
