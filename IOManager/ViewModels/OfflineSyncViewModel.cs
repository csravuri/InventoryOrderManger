using System.Collections.ObjectModel;
using System.Text;
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
			await FindWhatIsAlreadyPresentInReceiver();

			var items = await Connection.GetAll<ItemModel>(x => true);

			var jsonbytes = JsonSerializer.SerializeToUtf8Bytes(items);
			var base64String = Convert.ToBase64String(jsonbytes);

			ChunkDataAndShowQrCodes(base64String);
		}

		[RelayCommand]
		async Task Receive()
		{
			Status = "Receiving";
			await ShowWhatIsAlreadyPresentToSender();


		}

		async Task FindWhatIsAlreadyPresentInReceiver()
		{
			await Shell.Current.DisplayAlert("Important", "Open Receiver device and click Receive, and scan with sender device", "Ready");

			//var qrData = "{"Index":1,"DataText":"W3siTmFtZSI6Ikl0ZW1Nb2RlbCIsIkRhdGEiOiJZMkkxWlRJd09XWXRNalk1TUMwMFpEYzRMVGcwTm1RdE5HRXlNMkU1TXpreE1UYzRMR1V5WmpJMFkyVmxMVFkxT1dRdE5ERTNZeTFpTWpFeExUaGpPREkzTURVd1pHSmlaQT09In1d","Count":1}";

		}

		async Task ShowWhatIsAlreadyPresentToSender()
		{
			await Shell.Current.DisplayAlert("Important", "Show what is already present", "Ready");
			var existingDataBase64 = await GetExistingDataIds();

			ChunkDataAndShowQrCodes(existingDataBase64);

		}

		async Task<string> GetExistingDataIds()
		{
			var data = new List<ModelData>();

			var items = await Connection.GetAll<ItemModel>(x => true);
			if (items.Count > 0)
			{
				var itemIds = string.Join(",", items.Select(x => x.Id));

				data.Add(new ModelData
				{
					Name = nameof(ItemModel),
					Data = Convert.ToBase64String(Encoding.UTF8.GetBytes(itemIds))
				});
			}

			var orders = await Connection.GetAll<OrderHeaderModel>(x => true);
			if (orders.Count > 0)
			{
				var orderIds = string.Join(",", orders.Select(x => x.Id));

				data.Add(new ModelData
				{
					Name = nameof(OrderHeaderModel),
					Data = Convert.ToBase64String(Encoding.UTF8.GetBytes(orderIds))
				});
			}

			if (data.Count == 0)
			{
				Status = "Nothing here Start receiving";
				return string.Empty;
			}

			var jsonbytes = JsonSerializer.SerializeToUtf8Bytes(data);
			return Convert.ToBase64String(jsonbytes);
		}

		void ChunkDataAndShowQrCodes(string base64String)
		{
			QrCodeStrigs.Clear();
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

		//async Task<string> GetQrCodeImage(string text)
		//{
		//	var qrGenerator = new QRCodeGenerator();
		//	var qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.L);

		//	var qrCode = new PngByteQRCode(qrCodeData);
		//	var qrCodeImage = qrCode.GetGraphic(20, false);
		//	var imagePath = Path.Combine(FileSystem.CacheDirectory, $"{Guid.NewGuid()}.png");
		//	await File.WriteAllBytesAsync(imagePath, qrCodeImage);
		//	return imagePath;
		//}
	}

	public class QrCodeData
	{
		public int Index { get; set; }
		public string DataText { get; set; }
		public int Count { get; set; }
	}

	public class ModelData
	{
		public string Name { get; set; }
		public string Data { get; set; }
	}
}
