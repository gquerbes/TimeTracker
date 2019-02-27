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
            watch3.Start();
            //load ticket from DB
            var rawData = await RepliConnect.GetTickets();
            watch3.Stop();
            Debug.WriteLine($"****SERVER LOAD***** : {watch3.ElapsedMilliseconds}");
            watch3.Reset();

            watch2.Start();
            //save parent tasks as tasks
            if (rawData.Item1.d != null)
            {
                foreach (var projectRootObject in rawData.Item1.d)
                {
                    if (projectRootObject != null && projectRootObject.project != null)
                    {
                        watch3.Start();
                        if (!App.Database.Query<RepliconTask>($"{nameof(RepliconTask.uri)} = \"{projectRootObject?.project?.uri}\"").Any()
                        ) //if ticket not in database
                        {
                            watch3.Stop();
                            //Debug.WriteLine($"QueryTasks {watch.ElapsedMilliseconds}");
                            watch3.Reset();
                            //create task from project to allow time entry on parent tickets
                            watch3.Start();
                            var task = new RepliconTask();
                            task.description = projectRootObject.project.displayText;
                            task.displayText = projectRootObject.project.name;
                            task.name = projectRootObject.project?.name;
                            task.uri = projectRootObject.project.uri;
                            watch3.Stop();
                            //Debug.WriteLine($"create object {watch.ElapsedMilliseconds}");
                            watch3.Reset();

                            watch3.Start();
                            App.Database.SaveItem(task);
                            watch3.Stop();
                            //Debug.WriteLine($"SaveProjectAsTask {watch.ElapsedMilliseconds}");
                            watch3.Reset();

                        }
                    }
                }
            }
            watch2.Stop();
            Debug.WriteLine($"**********Total Time for Projects {watch2.ElapsedMilliseconds}***********");
            watch2.Reset();

            watch2.Start();
            //save  tasks
            if (rawData?.Item2.d != null)
                foreach (var item in rawData?.Item2.d)
                {
                    if (item?.childTasks != null)
                    {
                        foreach (var childtask in item?.childTasks)
                        {
                            watch3.Start();
                            if (!App.Database.Query<RepliconTask>($"{nameof(RepliconTask.uri)} = \"{childtask.Task.uri}\"").Any()
                            ) //if ticket not in database
                            {
                                watch3.Stop();
                            Debug.WriteLine($"QueryForTask: {watch3.ElapsedMilliseconds}");
                            watch3.Reset();


                                watch3.Start();
                                //save
                                App.Database.SaveItem(childtask.Task);
                                watch3.Stop();
                                Debug.WriteLine($"SaveTask: {watch3.ElapsedMilliseconds}");
                                watch3.Reset();

                            }
                        }
                    }
                        
                }

            watch2.Stop();
            Debug.WriteLine($"Total Time for Tasks {watch2.ElapsedMilliseconds}");

            watch1.Stop();
            Debug.WriteLine($"Total:  {watch1.ElapsedMilliseconds}");

            OnTicketLoadCompleted?.Invoke();
            return watch1.ElapsedMilliseconds.ToString();
        }
    }

}
     
