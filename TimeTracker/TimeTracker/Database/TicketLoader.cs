using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace TimeTracker.Database
{
    public class TicketLoader
    {
        static HttpClient client = new HttpClient();
        public static async void LoadData()
        {


            WebRequest req = WebRequest.Create(@"http://support.abas-usa.com/rest/api/2/search?jql=assignee=gquerbes");
            req.Method = "GET";
            req.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes("user:password"));

            WebResponse response = req.GetResponse();

            Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            Stream dataStream = response.GetResponseStream();

            StreamReader reader = new StreamReader(dataStream);

            var responseFromServer = reader.ReadToEnd();

            Console.WriteLine(responseFromServer);

            reader.Close();
            response.Close();


        }
    }
}
