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
		async Task LocalShare()
		{
			if (!File.Exists(ZipFilePath))
			{
				await Shell.Current.DisplayAlert("Error!", "Create backup before share", "Ok");
				return;
			}

			await Share.Default.RequestAsync(new ShareFileRequest
			{
				File = new ShareFile(ZipFilePath),
				Title = "Share or save file"
			});
		}

		[RelayCommand]
		async Task CloudShare()
		{
			await Shell.Current.DisplayAlert("Information!", "Coming soon", "Ok");
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
		//const string UploadAlert = "We use online file upload service https://gofile.io/welcome as a storage location to move your data from one device to other. Please read more about how they handle your data. Click Ok if agree.";
	}
}
