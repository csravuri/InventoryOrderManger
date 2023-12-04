using CommunityToolkit.Mvvm.ComponentModel;
using IOManager.Models;
using IOManager.Utils;

namespace IOManager.ViewModels
{
	public partial class ItemViewModel : ObservableObject
	{
		public ItemViewModel(ItemModel item)
		{
			Item = item;
		}

		[ObservableProperty]
		bool isSelected;

		public string ImagePath => Item.ImagePath == GlobalConstants.DefaultItemImage ? Item.ImagePath : Path.Combine(GlobalConstants.ImagesFolder, Item.ImagePath);

		public ItemModel Item { get; }
	}
}
