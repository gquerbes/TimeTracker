using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TimeTracker.Models.Replicon.RepliconReply;
using TimeTracker.Services;

namespace TimeTracker.Database
{
    /// <summary>
    /// Loads all available tickets fro JIRA into the app
    /// </summary>
    public class TicketLoader
    {
        static HttpClient client = new HttpClient();

        public delegate void TicketLoadCompleted();

        public static TicketLoadCompleted OnTicketLoadCompleted;

        public static Stopwatch watch3 = new Stopwatch();
        public static Stopwatch watch2 = new Stopwatch();
        public static Stopwatch watch1 = new Stopwatch();

        public static async Task<string> LoadData()
        {
         
            watch1.Start();
            //load ticket from DB
            var rawData = await RepliConnect.GetTickets();
            watch1.Stop();

            watch2.Start();
            foreach (var item in rawData)
            {
                //skip first line because it is the header

               
                var task = new RepliconTask();

                if (string.IsNullOrEmpty(item.TaskURI)) //is Project
                {
                   
                    if (!App.Database.Query<RepliconTask>($"{nameof(RepliconTask.uri)} = \"{item.ProjectURI}\"").Any()) // check to see if already in BD
                    {
                        task.description = item.ProjectName;
                        task.name = item.ProjectName;
                        task.uri = item.ProjectURI;
                        task.IsBillable = (!string.IsNullOrEmpty(item.BillingType)) && item.BillingType.Equals("Time And Material");

                    }
                    else continue;
                }
                else // is task
                {
                    if (!App.Database.Query<RepliconTask>($"{nameof(RepliconTask.uri)} = \"{item.TaskURI}\"").Any()) // check to see if already in BD
                    {
                        task.description = item.TaskCode;
                        task.name = item.TaskName;
                        task.uri = item.TaskURI;
                        task.IsBillable = (!string.IsNullOrEmpty(item.BillingType)) && item.BillingType.Equals("Time And Material");

                    }
                    else continue;
                }

                App.Database.SaveItem(task);
            }
            watch2.Stop();


            OnTicketLoadCompleted?.Invoke();

            return $"Watch 1 {watch1.ElapsedMilliseconds.ToString()}\nWatch2 {watch2.ElapsedMilliseconds}";
        }
    }

}
     
