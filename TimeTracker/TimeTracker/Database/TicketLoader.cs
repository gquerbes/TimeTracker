using System;
using System.Collections.Generic;
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


            WebRequest req = WebRequest.Create(@"http://support.abas-usa.com/rest/api/2/search?jql=assignee=gquerbes");
            req.Method = "GET"
            req.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes($"{Credentials.Username}:{Credentials.Password}"));

            WebResponse response = req.GetResponse();

            Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            Stream dataStream = response.GetResponseStream();

            StreamReader reader = new StreamReader(dataStream);

            var responseFromServer = reader.ReadToEnd();


            var text = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseFromServer);

            Console.WriteLine(text);

            reader.Close();
            response.Close();


        }
    }
}
