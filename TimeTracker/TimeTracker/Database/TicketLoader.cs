using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SQLitePCL;
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


        private static string[] currentURIS;

        private static bool Predicate(RepliconReportCSV arg)
        {
            

            if (string.IsNullOrEmpty(arg.TaskURI))// is project
            {
               return  !currentURIS.Any(x => x.Equals(arg?.ProjectURI));
            }
            else// is task
            {
                return !currentURIS.Any(x => x.Equals(arg?.TaskURI));
            }
        }

        public static async Task<string> LoadData(IProgress<double> progress)
        {

            //get list of all uri's on the database.
             currentURIS = App.Database.GetItems<RepliconTask>().Select(x => x.uri).ToArray();

             progress?.Report(.005);

            //load ticket from DB
            var rawData = await RepliConnect.GetTickets();

            progress?.Report(.01);


            var dataList = rawData.ToList();

            int rawDataCount = dataList.Count;

            //var dataList1 = dataList.Where(predicate: Predicate);


            

            double totalProcessed = 0;
            foreach (var item in dataList)
            {

                //update progress
                progress?.Report(Math.Round(totalProcessed++ / rawDataCount, 3));

                if (!Predicate(item))
                {
                    continue;
                }

                var task = new RepliconTask();


                if (string.IsNullOrEmpty(item.TaskURI)) //is Project
                {
                    task.description = item.ProjectName;
                    task.name = item.ProjectName;
                    task.uri = item.ProjectURI;
                }
                else // is task
                {
                    task.description = item.TaskCode;
                    task.name = item.TaskName;
                    task.uri = item.TaskURI;
                    
                }
                //set billable status
                task.IsBillable = (!string.IsNullOrEmpty(item.BillingType)) &&
                                  item.BillingType.Equals("Time And Material");

                App.Database.SaveItem(task);
            }

            progress?.Report(1);

            OnTicketLoadCompleted?.Invoke();

            return $"DONE";
        }

     
    }

}

