using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using InventoryOrderManger.Common;
using InventoryOrderManger.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InventoryOrderManger.Database
{
    public class DbManage
    {
        private DbConnection dbConnection = DbConnection.GetDbConnection();

        /// <summary>
        ///
        /// </summary>
        /// <returns>Path of file which contains data</returns>
        public async Task<string> ExportDbData()
        {
            string data = await GetDbData();

            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Database");

            Utils.MakeSureDirectoryExists(folderPath);

            string filePath = Path.Combine(folderPath, "InventoryOrderMangerDB.txt");

            File.WriteAllText(filePath, data);

            return filePath;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="filePath">Path of the file which contains data</param>
        /// <returns></returns>
        public async Task ImportDbData(string filePath)
        {
            string jsonData = File.ReadAllText(filePath);

            await LoadDbData(jsonData);
        }

        private async Task<string> GetDbData()
        {
            List<Item> items = await dbConnection.GetItems();
            List<OrderHeader> orderHeaders = await dbConnection.GetOrderHeaders();
            List<OrderLine> orderLines = await dbConnection.GetOrderLines();
            List<Sequence> sequences = await dbConnection.GetSequences();

            JObject jsonObject = new JObject();

            jsonObject.Add(nameof(Item), JToken.FromObject(items));
            jsonObject.Add(nameof(OrderHeader), JToken.FromObject(orderHeaders));
            jsonObject.Add(nameof(OrderLine), JToken.FromObject(orderLines));
            jsonObject.Add(nameof(Sequence), JToken.FromObject(sequences));

            return jsonObject.ToString();
        }

        private async Task LoadDbData(string JsonData)
        {
            JObject jsonObject = JsonConvert.DeserializeObject(JsonData) as JObject;

            List<Item> items = jsonObject[nameof(Item)].ToObject<List<Item>>();
            List<OrderHeader> orderHeaders = jsonObject[nameof(OrderHeader)].ToObject<List<OrderHeader>>();
            List<OrderLine> orderLines = jsonObject[nameof(OrderLine)].ToObject<List<OrderLine>>();
            List<Sequence> sequences = jsonObject[nameof(Sequence)].ToObject<List<Sequence>>();

            await dbConnection.InsertRecord(items);
            await dbConnection.InsertRecord(orderHeaders);
            await dbConnection.InsertRecord(orderLines);
            await dbConnection.InsertRecord(sequences);
        }
    }
}