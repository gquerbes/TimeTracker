using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace TimeTracker.Database
{
    /// <summary>
    /// Loads all available tickets fro JIRA into the app
    /// </summary>
    public class TicketLoader
    {
        static HttpClient client = new HttpClient();
        public static void LoadData()
        {
            //Tickets updated within the last x weeks
            var TimeRange = "jql=updated >= -12w";
            //list of fields that we want to sync "customfield_10571" is the replicon ID of each ticket
            var fieldsToShow = "fields=summary,key,customfield_10571";
            //Do not limit number of results
            var maxResults = "maxResults=-1";


            WebRequest req = WebRequest.Create($"{Credentials.URL}{TimeRange}&{fieldsToShow}&{maxResults}");
            req.Method = "GET";
            req.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes($"{Credentials.Username}:{Credentials.Password}"));
            WebResponse response = null;
            StreamReader reader = null;
            try
            {
                response = req.GetResponse();

                Stream dataStream = response.GetResponseStream();

                reader = new StreamReader(dataStream);

                var responseFromServer = reader.ReadToEnd();

                var JiraResult = JsonConvert.DeserializeObject<JiraResponse>(responseFromServer);

                foreach (var ticket in JiraResult.issues)
                {
                    App.Database.SaveItem(ticket);
                }
            }


            catch (Exception e)
            {
                Debug.WriteLine($"**{e.Data}==>{e.Message}");
            }
            finally
            {
                reader?.Close();
                response?.Close();
            }
            



        }
    }
}
