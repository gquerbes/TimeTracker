using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
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

        public static void LoadData()
        {
            //load ticket from DB
            var rawData = RepliConnect.GetTickets();

            if (rawData?.d != null)
                foreach (var item in rawData?.d)
                {
                    if (item?.childTasks != null)
                        foreach (var childtask in item?.childTasks)
                        {
                            if (!App.Database.Query<Ticket>($"repliconID = \"{childtask.Task.uri}\"").Any()
                            ) //if ticket not in database
                            {
                                //add to database
                                Ticket ticket = new Ticket();
                                ticket.Summary = childtask.Task.description;
                                ticket.repliconID = childtask.Task.uri;
                                ticket.key = childtask.Task.name;
                                //save
                                App.Database.SaveItem(ticket);
                            }
                        }
                }

            OnTicketLoadCompleted?.Invoke();
        }
    }

}
     
