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
        public delegate void TicketLoadCompleted();
        public static TicketLoadCompleted OnTicketLoadCompleted;


        private static string[] currentURIS;

        /// <summary>
        /// Returns TRUE if the element is NOT already in current DB
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
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

        public static async Task LoadData(IProgress<double> progress)
        {

             //get list of all uri's on the database.
             currentURIS = App.Database.GetItems<RepliconTask>().Select(x => x.uri).ToArray();

             //update progressbar
            progress?.Report(.010);

            //load ticket from DB
            var rawData = await RepliConnect.GetTickets();

            //update progressbar
            progress?.Report(.015);

            //cast to list to allow getting total count
            var dataList = rawData.ToList();
            //set local variable of total count
            int rawDataCount = dataList.Count;

            
            //counter for number of items itterated
            double totalProcessed = 0;
            foreach (var item in dataList)
            {

                //update progress
                progress?.Report(Math.Round(totalProcessed++ / rawDataCount, 3));

                //Skip item if already in DB
                if (!Predicate(item))continue;

                //create new task
                var task = new RepliconTask();

                //assign values
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

                //save to DB
                App.Database.SaveItem(task);
            }
            
            //update progress to 100%
            progress?.Report(1);

            //Invoke delegate for any work to be done after sync
            OnTicketLoadCompleted?.Invoke();

            return;
        }

     
    }

}

