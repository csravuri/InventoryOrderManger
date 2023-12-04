using System.IO.Compression;
using System.Text.Json;
using IOManager.Database;
using IOManager.Models;

namespace IOManager.Utils
{
	class FileTransferHelper
	{
		public FileTransferHelper(DbConnection connection)
		{
			Connection = connection;
		}

		public async Task<string> GetBackupZip()
		{
			var dbData = await GetDbData();
			var images = await GetImages();
			var dataFolder = CopyDataToTempFolder(dbData, images);

			var zipFileDestination = Path.Combine(GetATempFolder(), ZipFileName);
			ZipFile.CreateFromDirectory(dataFolder, zipFileDestination, CompressionLevel.Fastest, false);

			return zipFileDestination;
		}

		async Task<string> GetDbData()
		{
			var backup = new BackupModel
			{
				Items = await Connection.GetAll<ItemModel>(_ => true),
				OrderHeaders = await Connection.GetAll<OrderHeaderModel>(_ => true),
				OrderLines = await Connection.GetAll<OrderLineModel>(_ => true),
			};

			return JsonSerializer.Serialize(backup);
		}

		async Task<string[]> GetImages()
		{
			var items = await Connection.GetAll<ItemModel>(_ => true);
			return items.Where(x => x.ImagePath != GlobalConstants.DefaultItemImage).Select(x => x.ImagePath).ToArray();
		}

		string CopyDataToTempFolder(string dbData, string[] images)
		{
			var dataFolder = GetATempFolder();

			File.WriteAllText(Path.Combine(dataFolder, DataFileName), Base64Encode(dbData));

			foreach (var img in images.Where(x => File.Exists(Path.Combine(GlobalConstants.ImagesFolder, x))))
			{
				File.Copy(Path.Combine(GlobalConstants.ImagesFolder, img), Path.Combine(dataFolder, img));
			}

			return dataFolder;
		}

		string Base64Encode(string plainText)
		{
			var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
			return System.Convert.ToBase64String(plainTextBytes);
		}


		const string DataFileName = "Data.txt";
		string ZipFileName => $"IOManagerBackup{GlobalConstants.UniqueName}.zip";

		string GetATempFolder()
		{
			var temp = Path.Combine(FileSystem.CacheDirectory, GlobalConstants.UniqueName);
			Directory.CreateDirectory(temp);
			return temp;
		}
		public DbConnection Connection { get; }
	}

	class BackupModel
	{
		public List<ItemModel> Items { get; set; }
		public List<OrderHeaderModel> OrderHeaders { get; set; }
		public List<OrderLineModel> OrderLines { get; set; }
	}
}
