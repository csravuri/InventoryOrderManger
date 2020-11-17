using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using InventoryOrderManger.Common;
using InventoryOrderManger.Models;
using SQLite;

namespace InventoryOrderManger.Database
{
    public class DbConnection
    {
        private SQLiteAsyncConnection _coneection;
        public DbConnection()
        {
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Database");

            Utils.MakeSureDirectoryExists(folderPath);

            string dbPath = Path.Combine(folderPath, "InventoryOrderMangerDB.db3");

            _coneection = new SQLiteAsyncConnection(dbPath);

            CreateTable();

        }

        private async void CreateTable()
        {
            await _coneection.CreateTableAsync<Item>();
        }

        public void InsertItem(Item item)
        {
            _coneection.InsertAsync(item);
        }

        public void UpdateItem(Item item)
        {
            _coneection.UpdateAsync(item);
        }

        public void DeleteItem(Item item)
        {
            _coneection.DeleteAsync(item);
        }

        public async Task<List<Item>> GetItems()
        {
            return await _coneection.Table<Item>().ToListAsync();
        }
    }
}
