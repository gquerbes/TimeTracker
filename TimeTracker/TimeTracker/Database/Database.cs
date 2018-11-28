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

        private readonly SQLiteConnection database;

    public AppDatabase(string dbPath)
    {
        database = new SQLiteConnection(dbPath);
        database.CreateTable<TimeEntry>();
        database.CreateTable<Fields>();
        database.CreateTable<Ticket>();
    }


    public List<T> GetItems<T>() where T: DataObj, new()
    {
        return database.Table<T>().ToList();
    }

        public int SaveItem(DataObj item)
        {
            if (string.IsNullOrEmpty(item.Guid))
            {
                item.Guid = Guid.NewGuid().ToString();
                return database.Insert(item);
            }
            else
            {
                return database.Update(item);
            }
        }

        public int DeleteItem(DataObj item)
        {
            return database.Delete(item);
        }

    }
}
