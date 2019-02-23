using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RepliconIntegrator.Models;
using TimeTracker.Models.Replicon.RepliconReply;
using TimeTracker.Models.Replicon.RepliconRequest;
using TimeTracker.Models;
using TimeTracker.Models.Replicon.RepliconReply;

namespace TimeTracker.Services
{
    public class RepliConnect
    {
        public static RepliconTaskResponse GetTickets()
        {
            var user = GetUser();

            var userURI = JsonConvert.DeserializeObject<GetUser2Response>(user.ToString()).d.uri;

            var timesheetReply = GetTimesheetUri(userURI, DateTime.Today);
            var timesheetURI = JsonConvert.DeserializeObject<GetTimesheetForDate2Response>(timesheetReply.ToString());

            var y = timesheetURI.d.timesheet.uri;

            var projectsList = JsonConvert.DeserializeObject<GetPageOfProjectsFilteredByClientAndTextSearchResponse>(GetProjectsForTimesheet(y).ToString()).d.Select(x => x.project.uri).ToList();

            var tasks = JsonConvert.DeserializeObject<RepliconTaskResponse>(GetTaskFromProjects(projectsList).ToString());

            return tasks;


        }

        private static JToken GetProjectsForTimesheet(string TimesheetURI)
        {
            AppRequest req = new AppRequest();
            req.serviceURL = GetPageOfProjectsFilteredByClientAndTextSearchRequest.ServiceURL;
            var input = new GetPageOfProjectsFilteredByClientAndTextSearchRequest();
            input.pageSize = 1000;
            input.timesheetUri = TimesheetURI;
            req.Input = JObject.FromObject(input);
            return GetServerData(req);

        }
        
        private static JToken GetTaskFromProjects(List<string> projectURIs)
        {
            AppRequest req = new AppRequest();
            req.serviceURL = BulkGetDescendantTaskDetailsRequest.ServiceURL;
            var input = new BulkGetDescendantTaskDetailsRequest();
         
            int x = 0;
            foreach (var projectUrI in projectURIs)
            {
                input.parentUris.Add(projectUrI);
                //limit number of projects for now
                if (x++ > 20)
                {
                    break;
                }
            }

            req.Input = JObject.FromObject(input);

            return GetServerData(req);

        }

        private static JToken GetUser()
        {
            AppRequest req = new AppRequest();
            req.serviceURL = GetUser2Request.ServiceURL;
            var input = new GetUser2Request();
            input.user.loginName = "gquerbes";
            req.Input = JObject.FromObject(input);
            return  GetServerData(req);
        }

        private static JToken GetTimesheetUri(string userURI, DateTime date)
        {
            AppRequest req = new AppRequest();
            req.serviceURL = GetTimesheetForDate2Request.ServiceURL;
            var input = new GetTimesheetForDate2Request();
            input.SetDate(DateTime.Today);
            input.userUri = userURI;
            req.Input = JObject.FromObject(input);
            return GetServerData(req);

        }

        private static JToken GetTimesheet(string timesheetURI)
        {
            AppRequest req = new AppRequest();
            req.serviceURL = GetStandardTimesheet2Request.ServiceUrl;
            var input = new GetStandardTimesheet2Request();
            input.timesheetUri = timesheetURI;
            req.Input = JObject.FromObject(input);
            return GetServerData(req);
        }


        private static JToken GetServerData(AppRequest appRequest, string requestMethod ="POST" )
        {
            //ignore http errors
#warning This is due to self signed cert, should fix
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;


            WebRequest req = WebRequest.Create($"{Credentials.RepliConnectURL}/api/values/x");
            req.ContentType = "application/json";
            req.Method = requestMethod;
            using(var streamWriter = new StreamWriter(req.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(appRequest);

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            try
            {
                var httpResponse = (HttpWebResponse) req.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    return streamReader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
           
           
        }


  

       
    }
}
