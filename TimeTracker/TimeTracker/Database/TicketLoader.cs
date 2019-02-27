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

        public static async Task LoadData()
        {
            //load ticket from DB
            var rawData = await RepliConnect.GetTickets();

            //save parent tasks as tasks
            if (rawData.Item1.d != null)
            {
                foreach (var projectRootObject in rawData.Item1.d)
                {
                    if (projectRootObject != null && projectRootObject.project != null)
                    {
                        if (!App.Database.Query<RepliconTask>($"{nameof(RepliconTask.uri)} = \"{projectRootObject?.project?.uri}\"").Any()
                        ) //if ticket not in database
                        {
                            //create task from project to allow time entry on parent tickets
                            var task = new RepliconTask();
                            task.description = projectRootObject.project.displayText;
                            task.displayText = projectRootObject.project.name;
                            task.name = projectRootObject.project.name;
                            task.uri = projectRootObject.project.uri;
                            App.Database.SaveItem(task);
                        }
                    }
                }
            }


            //save  tasks
            if (rawData?.Item2.d != null)
                foreach (var item in rawData?.Item2.d)
                {
                    if (item?.childTasks != null)
                    {
                        foreach (var childtask in item?.childTasks)
                        {
                            if (!App.Database.Query<RepliconTask>($"{nameof(RepliconTask.uri)} = \"{childtask.Task.uri}\"").Any()
                            ) //if ticket not in database
                            {
                                //save
                                App.Database.SaveItem(childtask.Task);
                            }
                        }
                    }
                        
                }

            OnTicketLoadCompleted?.Invoke();
        }
    }

}
     
