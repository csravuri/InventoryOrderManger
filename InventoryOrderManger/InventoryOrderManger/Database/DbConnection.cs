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
            await _connection.CreateTableAsync<OrderHeader>();
            await _connection.CreateTableAsync<OrderLine>();
            await _connection.CreateTableAsync<Sequence>();
        }

        public async Task InsertRecord<E>(E item) where E : BaseModel
        {
            await _connection.InsertAsync(item);
        }

        public async Task UpdateRecord<E>(E item) where E : BaseModel
        {
            await _connection.UpdateAsync(item);
        }

        public async Task DeleteRecord<E>(E item) where E : BaseModel
        {
            await _connection.DeleteAsync(item);
        }
        public async Task InsertRecord<E>(List<E> items) where E : BaseModel
        {
            await _connection.InsertAllAsync(items);
        }

        public async Task UpdateRecord<E>(List<E> items) where E : BaseModel
        {
            await _connection.UpdateAllAsync(items);
        }

        public async Task DeleteRecord<E>(List<E> items) where E : BaseModel
        {
            foreach (E item in items)
            {
                await DeleteRecord<E>(item);
            }
        }


        public async Task<List<Item>> GetItems()
        {
            return await _connection.Table<Item>().ToListAsync();
        }
        public async Task<List<OrderHeader>> GetOrderHeaders()
        {
            return await _connection.Table<OrderHeader>().ToListAsync();
        }

        public async Task<List<OrderLine>> GetOrderLines()
        {
            return await _connection.Table<OrderLine>().ToListAsync();
        }

        public async Task<List<Sequence>> GetSequences()
        {
            return await _connection.Table<Sequence>().ToListAsync();
        }

    }
}
