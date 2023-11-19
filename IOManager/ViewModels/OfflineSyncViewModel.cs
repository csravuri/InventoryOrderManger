using System.Collections.ObjectModel;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IOManager.Database;
using IOManager.Models;
using QRCoder;

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

			await ChunkDataAndAddQrCodes(base64String);
		}

		[RelayCommand]
		void Receive()
		{
			Status = "Receive";
		}

		async Task ChunkDataAndAddQrCodes(string base64String)
		{
			QrCodeStrigs.Clear();
			const int eachQrMaxLength = 2000;
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
				QrCodeStrigs.Add(await GetQrCodeImage(JsonSerializer.Serialize(qrData)));
			}
		}

		async Task<string> GetQrCodeImage(string text)
		{
			var qrGenerator = new QRCodeGenerator();
			var qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.L);

			var qrCode = new PngByteQRCode(qrCodeData);
			var qrCodeImage = qrCode.GetGraphic(20, false);
			var imagePath = Path.Combine(FileSystem.CacheDirectory, $"{Guid.NewGuid()}.png");
			await File.WriteAllBytesAsync(imagePath, qrCodeImage);
			return imagePath;
		}
	}

	public class QrCodeData
	{
		public int Index { get; set; }
		public string DataText { get; set; }
		public int Count { get; set; }
	}
}
