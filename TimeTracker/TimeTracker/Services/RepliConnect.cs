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

        private static string UserURI;

        private static string TimesheetURI;


        #region Public Methods
        public static BulkGetDescendantTaskDetailsResponse GetTickets()
        {
            var userReply = GetUser();

            UserURI = JsonConvert.DeserializeObject<GetUser2Response>(userReply.ToString()).d.uri;

            var timesheetReply = GetTimesheetUri(UserURI, DateTime.Today);

            TimesheetURI = JsonConvert.DeserializeObject<GetTimesheetForDate2Response>(timesheetReply.ToString()).d.timesheet.uri;

            var projectsList = JsonConvert.DeserializeObject<GetPageOfProjectsFilteredByClientAndTextSearchResponse>(GetProjectsForTimesheet(TimesheetURI).ToString()).d.Select(x => x.project.uri).ToList();

            //TODO: Pull ops overhead and other ticket

            var tasks = JsonConvert.DeserializeObject<Models.Replicon.RepliconReply.BulkGetDescendantTaskDetailsResponse>(GetTaskFromProjects(projectsList).ToString());

            return tasks;

        }

        public static bool SubmitTimesheet()
        {
            if (string.IsNullOrEmpty(TimesheetURI)) return false;

            //pull current timesheet
            var timesheet = GetTimesheet(TimesheetURI);
            GetStandardTimesheet2Reply currentTimesheet = timesheet.ToObject<GetStandardTimesheet2Reply>();
            //copy values over to new timesheet submission

            //add latest values

            //submit

            return true;
        }

        public static void CopyAndAdd()
        {
            GetStandardTimesheet2Reply currentTS = new GetStandardTimesheet2Reply();

            PutStandardTimesheet2Request newTS = new PutStandardTimesheet2Request();

            newTS.timesheet.target.uri = TimesheetURI;

            foreach (var row in currentTS.d.rows)
            {
              //  newTS.timesheet.rows.Add(new Row(row));
            }

        }
        #endregion

        #region Base Replicon Communication


        private static JToken GetProjectsForTimesheet(string TimesheetURI)
        {
            AppRequest req = new AppRequest();
            req.serviceURL = GetPageOfProjectsFilteredByClientAndTextSearchRequest.ServiceURL;
            var input = new GetPageOfProjectsFilteredByClientAndTextSearchRequest();
            input.pageSize = 100000;
            input.timesheetUri = TimesheetURI;
            req.Input = JObject.FromObject(input);
            return GetServerData(req);

        }
        
        private static JToken GetTaskFromProjects(List<string> projectURIs)
        {
            AppRequest req = new AppRequest();
            req.serviceURL = Models.Replicon.RepliconRequest.BulkGetDescendantTaskDetailsRequest.ServiceURL;
            var input = new Models.Replicon.RepliconRequest.BulkGetDescendantTaskDetailsRequest();
         
            foreach (var projectUrI in projectURIs)
            {
                input.parentUris.Add(projectUrI);
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


        #endregion

    }
}
