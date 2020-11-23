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
        private SQLiteAsyncConnection _connection;
        private static DbConnection _dbConnection;
        private DbConnection()
        {
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Database");

            Utils.MakeSureDirectoryExists(folderPath);

            string dbPath = Path.Combine(folderPath, "InventoryOrderMangerDB.db3");

            _connection = new SQLiteAsyncConnection(dbPath);

            CreateTable();

        }

        public static DbConnection GetDbConnection()
        {
            if(_dbConnection == null)
            {
                _dbConnection = new DbConnection();
            }

            return _dbConnection;
        }

        private async void CreateTable()
        {
            await _connection.CreateTableAsync<Item>();
        }

        public void InsertItem(Item item)
        {
            _connection.InsertAsync(item);
        }

        public void UpdateItem(Item item)
        {
            _connection.UpdateAsync(item);
        }

        public void DeleteItem(Item item)
        {
            _connection.DeleteAsync(item);
        }

        public async Task<List<Item>> GetItems()
        {
            return await _connection.Table<Item>().ToListAsync();
        }
    }
}
