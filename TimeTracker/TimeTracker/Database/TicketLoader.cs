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

        public static Stopwatch watch3 = new Stopwatch();
        public static Stopwatch watch2 = new Stopwatch();
        public static Stopwatch watch1 = new Stopwatch();


        public static async Task<string> LoadData(IProgress<double> progress)
        {
           var currentURIS =  App.Database.GetItems<RepliconTask>().Select(x => x.uri);


           if (progress != null)
               progress.Report(.01);



            watch1.Start();
               //load ticket from DB
               var rawData = await RepliConnect.GetTickets();
               watch1.Stop();
               Debug.WriteLine($"Create task object {watch1.ElapsedMilliseconds}ms");
               watch1.Reset();

               if (progress != null)
                   progress.Report(.05);

                watch1.Start();
               var cleanedLoad = rawData.Where(x => !currentURIS.Contains(x.TaskURI));
               watch1.Stop();
               Debug.WriteLine($"remove duplicates {watch1.ElapsedMilliseconds}ms");
               watch1.Reset();



               watch1.Start();
               watch2.Start();
               double itemNumber = 1;
               var repliconReportCsvs = cleanedLoad as RepliconReportCSV[] ?? cleanedLoad.ToArray();
               int totalInCleanedLoad = repliconReportCsvs.Count();
               foreach (var item in repliconReportCsvs)
               {

                   if (progress != null)
                       progress.Report(itemNumber++ / totalInCleanedLoad);

                    watch1.Stop();
                   Debug.WriteLine($"Enter loop of foreach {watch1.ElapsedMilliseconds}ms");

                   watch3.Start();
                   var task = new RepliconTask();
                   watch3.Stop();
                   Debug.WriteLine($"Create task object {watch3.ElapsedMilliseconds}ms");
                   watch3.Reset();


                   if (string.IsNullOrEmpty(item.TaskURI)) //is Project
                   {
                       watch3.Start();
                       if (!App.Database.Query<RepliconTask>($"{nameof(RepliconTask.uri)} = \"{item.ProjectURI}\"")
                           .Any()) // check to see if already in BD
                       {
                           watch3.Stop();
                           Debug.WriteLine($"Query local DB {watch3.ElapsedMilliseconds}ms");
                           watch3.Reset();

                           watch3.Start();
                           task.description = item.ProjectName;
                           task.name = item.ProjectName;
                           task.uri = item.ProjectURI;
                           task.IsBillable = (!string.IsNullOrEmpty(item.BillingType)) &&
                                             item.BillingType.Equals("Time And Material");
                           watch3.Stop();
                           Debug.WriteLine($"set object properties {watch3.ElapsedMilliseconds}ms");
                           watch3.Reset();

                       }
                       else continue;
                   }
                   else // is task
                   {
                       if (!App.Database.Query<RepliconTask>($"{nameof(RepliconTask.uri)} = \"{item.TaskURI}\"").Any()
                       ) // check to see if already in BD
                       {
                           task.description = item.TaskCode;
                           task.name = item.TaskName;
                           task.uri = item.TaskURI;
                           task.IsBillable = (!string.IsNullOrEmpty(item.BillingType)) &&
                                             item.BillingType.Equals("Time And Material");

                       }
                       else continue;
                   }

                   watch3.Start();
                   App.Database.SaveItem(task);
                   watch3.Stop();
                   Debug.WriteLine($"Save to DB{watch3.ElapsedMilliseconds}ms");
                   Debug.WriteLine("----------------------");

               }

               watch2.Stop();


               OnTicketLoadCompleted?.Invoke();

           return $"DONE";
        }
    }

}
     
