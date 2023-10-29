using CommunityToolkit.Mvvm.ComponentModel;
using IOManager.Models;

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

		public ItemModel Item { get; }
	}
}
