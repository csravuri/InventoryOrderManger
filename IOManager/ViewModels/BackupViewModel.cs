using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IOManager.Database;
using IOManager.Utils;

namespace IOManager.ViewModels
{
	public partial class BackupViewModel : ObservableObject
	{
		public BackupViewModel(DbConnection connection)
		{
			Connection = connection;
		}

		[RelayCommand]
		async Task Create()
		{
			try
			{
				Status = "Please wait, we are creating backup";
				var helper = new FileTransferHelper(Connection);
				ZipFilePath = await helper.GetBackupZip();
				Status = "File Created, Ready to share";
			}
			catch (Exception ex)
			{
				Status = ex.ToString();
			}

		}

		[RelayCommand]
		void LocalShare()
		{
			if (!File.Exists(ZipFilePath))
			{
				Shell.Current.DisplayAlert("Error!", "Create backup before share", "Ok");
				return;
			}

			Share.Default.RequestAsync(new ShareFileRequest
			{
				File = new ShareFile(ZipFilePath),
				Title = "Share or save file"
			});
		}

		[RelayCommand]
		void CloudShare()
		{
		}

		[RelayCommand]
		async Task Done()
		{
			Status = string.Empty;
			if (File.Exists(ZipFilePath))
			{
				File.Delete(ZipFilePath);
			}
			ZipFilePath = string.Empty;
			await Shell.Current.GoToAsync("..");
		}

		[ObservableProperty]
		string status;

		[ObservableProperty]
		string fileName;

		[ObservableProperty]
		string zipFilePath;

		partial void OnZipFilePathChanged(string value)
		{
			FileName = string.IsNullOrWhiteSpace(value) ? string.Empty : Path.GetFileName(ZipFilePath);
		}

		public DbConnection Connection { get; }
	}
}
