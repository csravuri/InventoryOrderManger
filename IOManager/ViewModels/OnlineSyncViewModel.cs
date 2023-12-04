using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IOManager.Database;
using IOManager.Utils;

namespace IOManager.ViewModels
{
	public partial class OnlineSyncViewModel : ObservableObject
	{
		public OnlineSyncViewModel(DbConnection connection)
		{
			Connection = connection;
		}

		[RelayCommand]
		async Task Send()
		{
			var helper = new FileTransferHelper(Connection);
			var zipFile = await helper.GetBackupZip();
		}

		[RelayCommand]
		void Receive()
		{
		}

		public string OnlineSyncInfo => SyncAlert;

		public DbConnection Connection { get; }

		const string SyncAlert = "We use online file upload service https://gofile.io/welcome as a storage location to move your data from one device to other. Please read more about how they handle your data. If you are using online sync feature we assume you are happy with this.";
	}
}
