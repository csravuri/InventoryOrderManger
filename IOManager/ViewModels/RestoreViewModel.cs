using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IOManager.Database;
using IOManager.Utils;

namespace IOManager.ViewModels
{
	public partial class RestoreViewModel : ObservableObject
	{
		public DbConnection Connection { get; }

		public RestoreViewModel(DbConnection connection)
		{
			Connection = connection;
		}

		[RelayCommand]
		async Task ImportLocalFile()
		{
			var file = await FilePicker.Default.PickAsync();
			if (file == null)
			{
				return;
			}

			var helper = new FileTransferHelper(Connection);
			await helper.RestoreZip(file.FullPath);
		}

		[RelayCommand]
		async Task ImportCloudFile()
		{
			await Shell.Current.DisplayAlert("Information!", "Coming soon", "Ok");
		}
	}
}
