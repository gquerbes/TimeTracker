using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SQLite;
using TimeTracker.Models;
using TimeTracker.Models.Replicon.RepliconReply;
using Task = System.Threading.Tasks.Task;

namespace TimeTracker.Database
{
    /// <summary>
    /// Represents the Apps database to used to persist data 
    /// </summary>
    public class AppDatabase

    {

        private readonly SQLiteConnection database;

        /// <summary>
        /// Creates the app database
        /// </summary>
        /// <param name="dbPath"></param>
        public AppDatabase(string dbPath)
        {
            database = new SQLiteConnection(dbPath);
            database.CreateTable<Models.Replicon.RepliconReply.RepliconTask>();
            database.CreateTable<TimeEntry>();
        }


        public List<T> GetItems<T>() where T : DataObj, new()
        {
            return database.Table<T>().ToList();
        }

        public List<T> Query<T>(string Query) where T : DataObj, new()
        {
           return database.Query<T>($"SELECT * FROM {database.GetMapping(typeof(T)).TableName} WHERE {Query}");
        }

        public Models.Replicon.RepliconReply.RepliconTask FindRepliconTaskByUri(string TaskURI)
        {
            var foundTask = Query<RepliconTask>($"{nameof(RepliconTask.uri)} = \"{TaskURI}\"").FirstOrDefault();
            if (foundTask == null)
            {
                foundTask = Query<RepliconTask>($"{nameof(RepliconTask.ProjectURI)} = \"{TaskURI}\"").FirstOrDefault();
            }

            return foundTask;
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
