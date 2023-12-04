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

			Directory.Delete(dataFolder, true);
			return zipFileDestination;
		}

		public async Task RestoreZip(string fileFullPath)
		{
			if (!File.Exists(fileFullPath))
			{
				return;
			}

			var destination = GetATempFolder();
			ZipFile.ExtractToDirectory(fileFullPath, destination);

			var dbDataJson = Base64Decode(File.ReadAllText(Path.Combine(destination, DataFileName)));
			var models = JsonSerializer.Deserialize<BackupModel>(dbDataJson);
			if (models == null)
			{
				return;
			}

			await Save(models);
			CopyImages(destination);
		}

		#region Backup

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

		#endregion

		#region Restore

		string Base64Decode(string base64EncodedData)
		{
			var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
			return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
		}

		async Task Save(BackupModel models)
		{
			if (models.Items?.Count != 0)
			{
				await CreateOrUpdateItem(models.Items);
			}

			if (models.OrderHeaders?.Count != 0)
			{
				await CreateOrUpdateHeader(models.OrderHeaders);
			}

			if (models.OrderLines?.Count != 0)
			{
				await CreateOrUpdateLine(models.OrderLines);
			}
		}

		async Task CreateOrUpdateItem(List<ItemModel> items)
		{
			foreach (var item in items)
			{
				var dbItem = await Connection.Get<ItemModel>(item.Id);
				if (dbItem != null)
				{
					await Connection.Update(item);
				}
				else
				{
					await Connection.Create(item);
				}
			}
		}

		async Task CreateOrUpdateHeader(List<OrderHeaderModel> orderHeaders)
		{
			foreach (var header in orderHeaders)
			{
				var dbItem = await Connection.Get<OrderHeaderModel>(header.Id);
				if (dbItem != null)
				{
					await Connection.Update(header);
				}
				else
				{
					await Connection.Create(header);
				}
			}
		}

		async Task CreateOrUpdateLine(List<OrderLineModel> orderLines)
		{
			await Connection.Create(orderLines);
		}

		void CopyImages(string destination)
		{
			var files = Directory.GetFiles(destination, "*.jpg");
			foreach (var file in files)
			{
				File.Copy(file, Path.Combine(destination, Path.GetFileName(file)), true);
			}
		}

		#endregion

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
