using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using TimeTracker.Models;

namespace TimeTracker.Database
{
    public class AppDatabase

    {
    private readonly SQLiteAsyncConnection database;

    public AppDatabase(string dbPath)
    {
        database = new SQLiteAsyncConnection(dbPath);
        database.CreateTableAsync<TimeEntry>().Wait();
        //database.CreateTableAsync<Ticket>().Wait();
    }


    public Task<List<T>> GetItemsAsync<T>() where T: DataObj, new()
    {
        return database.Table<T>().ToListAsync();
    }

        public Task<int> SaveItemAsync(DataObj item)
        {
            if (string.IsNullOrEmpty(item.Guid))
            {
                item.Guid = Guid.NewGuid().ToString();
                return database.InsertAsync(item);
            }
            else
            {
                return database.UpdateAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(DataObj item)
        {
            return database.DeleteAsync(item);
        }

    }
}
