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
    public class TicketLoader
    {
        static HttpClient client = new HttpClient();
        public static void LoadData()
        {

            var TimeRange = "jql=updated >= -12w";
            var fieldsToShow = "fields=summary,key,customfield_10571";
            var maxResults = "maxResults=-1";


            WebRequest req = WebRequest.Create($"{@"http://support.abas-usa.com/rest/api/2/search?"}{TimeRange}&{fieldsToShow}&{maxResults}");
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

                var text = JsonConvert.DeserializeObject<JiraResponse>(responseFromServer);
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
