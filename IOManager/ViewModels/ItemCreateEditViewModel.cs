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
		string imagePath = GlobalConstants.DefaultItemImage;

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
					ImagePath = Path.GetFileName(ImagePath)
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
			ImagePath = GlobalConstants.DefaultItemImage;
		}

		[RelayCommand]
		async Task ImageTap()
		{
			if (ImagePath == GlobalConstants.DefaultItemImage)
			{
				var resultWhenDefaultImage = await Shell.Current.DisplayActionSheet(ImageCaption, BackCaption, string.Empty, GalleryCaption, CameraCaption);
				await GetImage(resultWhenDefaultImage);
				return;
			}

			var result = await Shell.Current.DisplayActionSheet(ImageCaption, BackCaption, RemoveCaption, GalleryCaption, CameraCaption);
			if (result == RemoveCaption)
			{
				ImagePath = GlobalConstants.DefaultItemImage;
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
				var destination = GetImageFullPath(capturedImagePath);
				File.Move(capturedImagePath, destination);
				ImagePath = destination;
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
				var destination = GetImageFullPath(pickedImagePath, true);
				File.Copy(pickedImagePath, destination);
				ImagePath = destination;
			}
		}

		string GetImageFullPath(string sourcePath, bool newFileFordestination = false)
		{
			if (!Directory.Exists(ImagesSubFolderPath))
			{
				Directory.CreateDirectory(ImagesSubFolderPath);
			}

			var fileName = Path.GetFileNameWithoutExtension(sourcePath);
			var extention = Path.GetExtension(sourcePath);
			if (newFileFordestination)
			{
				fileName = GlobalConstants.UniqueName;
			}

			return Path.Combine(ImagesSubFolderPath, fileName + extention);
		}

		void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
		{
			if (query.TryGetValue(GlobalConstants.ItemSearchText, out var searchTextValue) && searchTextValue is string itemSearchText)
			{
				ItemName = itemSearchText;
			}
			else if (query.TryGetValue(GlobalConstants.ItemUpdate, out var modelValue) && modelValue is ItemViewModel viewModel)
			{
				var item = viewModel.Item;
				ItemName = item.ItemName;
				WholeSalePrice = item.WholeSalePrice;
				RetailSalePrice = item.RetailSalePrice;
				PurchasePrice = item.PurchasePrice;
				StockQuantity = item.StockQuantity;
				Description = item.Description;
				ImagePath = item.ImagePath == GlobalConstants.DefaultItemImage ? item.ImagePath : Path.Combine(ImagesSubFolderPath, item.ImagePath);
				modelId = item.Id;

				Title = UpdateItemCaption;
				IsCreate = false;
			}
		}

		const string BackCaption = "Back";
		const string RemoveCaption = "Remove";
		const string GalleryCaption = "Gallery";
		const string CameraCaption = "Camera";
		const string ImageCaption = "Image";
		const string CreateItemCaption = "Create Item";
		const string UpdateItemCaption = "Update Item";

		DbConnection Connection { get; }
		readonly string ImagesSubFolderPath = GlobalConstants.ImagesFolder;
		Guid modelId;
	}
}
