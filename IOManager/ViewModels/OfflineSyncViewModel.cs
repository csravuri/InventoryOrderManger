using System.Collections.ObjectModel;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IOManager.Database;
using IOManager.Models;

namespace IOManager.ViewModels
{
	public partial class OfflineSyncViewModel : ObservableObject
	{
		public OfflineSyncViewModel(DbConnection connection)
		{
			Connection = connection;
			QrCodeStrigs = new ObservableCollection<string>();
		}

		public ObservableCollection<string> QrCodeStrigs { get; }

		DbConnection Connection { get; }



		[ObservableProperty]
		string status;

		[RelayCommand]
		async Task Send()
		{
			var items = await Connection.GetAll<ItemModel>(x => true);

			var jsonbytes = JsonSerializer.SerializeToUtf8Bytes(items);
			var base64String = Convert.ToBase64String(jsonbytes);

			QrCodeStrigs.Add(base64String);
		}

		[RelayCommand]
		void Receive()
		{
			Status = "Receive";
		}
	}
}
