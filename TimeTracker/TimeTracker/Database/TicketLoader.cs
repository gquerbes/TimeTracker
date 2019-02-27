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
            int x = 0;
            foreach (var item in rawData)
            {
                //skip first line because it is the header
                if(x++ == 0) continue;

                string projectName = item.ElementAt(0)?.ToString();
                string taskName = item.ElementAt(1)?.ToString();
                string billingType = item.ElementAt(2)?.ToString();
                string taskCode = item.ElementAt(3)?.ToString();
                string taskURi = item.ElementAt(4)?.ToString();
                string projectUri = item.ElementAt(5)?.ToString();
                var task = new RepliconTask();

                if (string.IsNullOrEmpty(taskURi)) //is Project
                {
                   
                    if (!App.Database.Query<RepliconTask>($"{nameof(RepliconTask.uri)} = \"{projectUri}\"").Any()) // check to see if already in BD
                    {
                        task.description = projectName;
                        task.name = projectName;
                        task.uri = projectUri;
                        task.IsBillable = (!string.IsNullOrEmpty(billingType)) && billingType.Equals("Time And Material");

                    }
                    else continue;
                }
                else // is task
                {
                    if (!App.Database.Query<RepliconTask>($"{nameof(RepliconTask.uri)} = \"{taskURi}\"").Any()) // check to see if already in BD
                    {
                        task.description = taskCode;
                        task.name = taskName;
                        task.uri = taskURi;
                        task.IsBillable = (!string.IsNullOrEmpty(billingType)) && billingType.Equals("Time And Material");

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
     
