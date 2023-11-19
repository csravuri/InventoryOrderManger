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
			QrCodeStrigs.Clear();
			var items = await Connection.GetAll<ItemModel>(x => true);

			var jsonbytes = JsonSerializer.SerializeToUtf8Bytes(items);
			var base64String = Convert.ToBase64String(jsonbytes);

			const int eachQrMaxLength = 100;
			int indx = 1;
			var chunks = base64String.Chunk(eachQrMaxLength);
			foreach (var item in chunks.Select(x => new string(x)))
			{
				var qrData = new QrCodeData
				{
					Index = indx++,
					DataText = item,
					Count = chunks.Count()
				};
				QrCodeStrigs.Add(JsonSerializer.Serialize(qrData));
			}
		}

		[RelayCommand]
		void Receive()
		{
			Status = "Receive";
		}
	}

	public class QrCodeData
	{
		public int Index { get; set; }
		public string DataText { get; set; }
		public int Count { get; set; }
	}
}
