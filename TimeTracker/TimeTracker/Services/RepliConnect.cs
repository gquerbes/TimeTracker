using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RepliconIntegrator.Models;
using TimeTracker.Helpers;
using TimeTracker.Models.Replicon.RepliconReply;
using TimeTracker.Models.Replicon.RepliconRequest;
using TimeTracker.Models;
using TimeTracker.Models.Replicon.RepliconReply;
using TimeTracker.ViewModels;
using Date = TimeTracker.Models.Replicon.RepliconReply.Date;
using Timesheet = TimeTracker.Models.Replicon.RepliconReply.Timesheet;

namespace TimeTracker.Services
{
    public class RepliConnect
    {

        private static string UserURI;

        private static string TimesheetURI;


        #region Public Methods
        /// <summary>
        /// Returns tuple of projects and tasks responses
        /// </summary>
        /// <returns></returns>
        public static async Task<Tuple<GetPageOfProjectsFilteredByClientAndTextSearchResponse, BulkGetDescendantTaskDetailsResponse>> GetTickets()
        {
            var userReply = await GetUser();

            UserURI = JsonConvert.DeserializeObject<GetUser2Response>(userReply.ToString()).d.uri;

            var timesheetReply = await GetTimesheetUri(UserURI, DateTime.Today);

            TimesheetURI = JsonConvert.DeserializeObject<GetTimesheetForDate2Response>(timesheetReply.ToString()).d.timesheet.uri;

            var projectsList = JsonConvert.DeserializeObject<GetPageOfProjectsFilteredByClientAndTextSearchResponse>((await GetProjectsForTimesheet(TimesheetURI)).ToString());

            var projectListURIs = projectsList.d.Select(x => x.project.uri).ToList();

            //TODO: Pull ops overhead and other ticket
            var taskReply = await GetTaskFromProjects(projectListURIs);

            var tasks = JsonConvert.DeserializeObject<BulkGetDescendantTaskDetailsResponse>(taskReply.ToString());


            return Tuple.Create(projectsList, tasks);

        }

        private static PutStandardTimesheet2Request CreateTimesheetSubmissionForCurrentTimesheet()
        {
            /*
            * do beforehand and save values
            */
            var userReply = GetUser();

            UserURI = JsonConvert.DeserializeObject<GetUser2Response>(userReply.ToString()).d.uri;

            var timesheetReply = GetTimesheetUri(UserURI, DateTime.Today);

            TimesheetURI = JsonConvert.DeserializeObject<GetTimesheetForDate2Response>(timesheetReply.ToString()).d.timesheet.uri;
            /*
          * do beforehand and save values
          */

            GetStandardTimesheet2Reply currentTS = JsonConvert.DeserializeObject<GetStandardTimesheet2Reply>(GetTimesheet(TimesheetURI).ToString());

            PutStandardTimesheet2Request newTS = new PutStandardTimesheet2Request();

            //copy URI of current Timesheet
            newTS.timesheet.target.uri = TimesheetURI;
            //set employee
            newTS.timesheet.target.user.uri = currentTS.d.uri;
            //set date
            newTS.timesheet.target.date.day = DateTime.Today.Day;
            newTS.timesheet.target.date.month = DateTime.Today.Month;
            newTS.timesheet.target.date.year = DateTime.Today.Year;
            //copy current rows
            foreach (var dRow in currentTS.d.rows)
            {
                dRow.task.parameterCorrelationId = "TimeTrackerApp";
                newTS.timesheet.rows.Add(dRow);
            }
            //set value of whatever this means
            newTS.timesheet.noticeExplicitlyAccepted = "0";

            return newTS;
        }


        public static void SubmitTimesheet(List<TimeEntryParent> timeEntries)
        {

            var timesheetSubmission = CreateTimesheetSubmissionForCurrentTimesheet();
            foreach (var timeEntry in timeEntries)//for each entry to be submitted
            {
                foreach (var timesheetRow in timesheetSubmission.timesheet.rows) // foreach timesheet row currently on timesheet
                {
                    if (timesheetRow.task.uri.Equals(timeEntry.RepliconTicketID) // current row matches current entry project
                        && ((string.IsNullOrEmpty(timesheetRow.billingRate?.uri) && !timeEntry.IsBillable) // both billable
                        || (!string.IsNullOrEmpty(timesheetRow.billingRate?.uri) && timeEntry.IsBillable))) //neither billable
                    {
                        var newCell = CreateCellFromEntry(timeEntry);
                        //find entry on row with same date
                        var existingEntry = timesheetRow.cells.FirstOrDefault(x => x.date.day.Equals(newCell.date.day));
                        //if entry exists for same date replace, else add to list
                        if (existingEntry != null)
                        {
                            timesheetRow.cells.Remove(existingEntry);
                        }
                        
                        timesheetRow.cells.Add(newCell);
                        timeEntry.ExistsOnTimeSheet = true;
                        break;
                    }
                    
                }
                //iterated through all current rows and not match found, make a new row
                if (!timeEntry.ExistsOnTimeSheet)
                {
                    //create new row
                    Row newRow = new Row();

                    if (timeEntry.IsBillable)
                    {
                        newRow.billingRate = new BillingRate() { uri = "urn:replicon:project-specific-billing-rate" };
                    }

                    newRow.task.uri = timeEntry.Ticket.uri;

                    var newCell = CreateCellFromEntry(timeEntry);

                    newRow.cells.Add(newCell);
                    timesheetSubmission.timesheet.rows.Add(newRow);
                }

            }


            PutTimesheet(timesheetSubmission);
        }

        private static Cell CreateCellFromEntry(TimeEntryParent timeEntry)
        {
            //new cell for entry
            var newCell = new Cell();
            //comments
            newCell.comments = timeEntry.Comments;
            //duration
            var roundedTime = timeEntry.TotalTime.RoundToNearestMinutes(15);
            newCell.duration.hours = roundedTime.Hours.ToString();
            newCell.duration.minutes = roundedTime.Minutes.ToString();
            newCell.duration.seconds = "0";
            newCell.duration.milliseconds = "0";
            newCell.duration.microseconds = "0";
            //date
            newCell.date.day = timeEntry.Date.Day;
            newCell.date.month = timeEntry.Date.Month;
            newCell.date.year = timeEntry.Date.Year;

            return newCell;
        }
        #endregion

        #region Base Replicon Communication


        private static async Task<JToken> GetProjectsForTimesheet(string TimesheetURI)
        {
            AppRequest req = new AppRequest();
            req.serviceURL = GetPageOfProjectsFilteredByClientAndTextSearchRequest.ServiceURL;
            var input = new GetPageOfProjectsFilteredByClientAndTextSearchRequest();
            input.pageSize = 100000;
            input.timesheetUri = TimesheetURI;
            req.Input = JObject.FromObject(input);
            return await GetServerData(req);

        }

        private static async Task<JToken> GetTaskFromProjects(List<string> projectURIs)
        {
            AppRequest req = new AppRequest();
            req.serviceURL = Models.Replicon.RepliconRequest.BulkGetDescendantTaskDetailsRequest.ServiceURL;
            var input = new Models.Replicon.RepliconRequest.BulkGetDescendantTaskDetailsRequest();

            foreach (var projectUrI in projectURIs)
            {

                input.parentUris.Add(projectUrI);
            }

            req.Input = JObject.FromObject(input);

            return await GetServerData(req);

        }

        private static async Task<JToken> GetUser()
        {
            AppRequest req = new AppRequest();
            req.serviceURL = GetUser2Request.ServiceURL;
            var input = new GetUser2Request();
            input.user.loginName = "gquerbes";
            req.Input = JObject.FromObject(input);
            return await GetServerData(req);
        }

        private static async Task<JToken> GetTimesheetUri(string userURI, DateTime date)
        {
            AppRequest req = new AppRequest();
            req.serviceURL = GetTimesheetForDate2Request.ServiceURL;
            var input = new GetTimesheetForDate2Request();
            input.SetDate(DateTime.Today);
            input.userUri = userURI;
            req.Input = JObject.FromObject(input);
            var response = GetServerData(req);

            return await response;
        }

        private static async Task<JToken> GetTimesheet(string timesheetURI)
        {
            AppRequest req = new AppRequest();
            req.serviceURL = GetStandardTimesheet2Request.ServiceUrl;
            var input = new GetStandardTimesheet2Request();
            input.timesheetUri = timesheetURI;
            req.Input = JObject.FromObject(input);
            var response = await GetServerData(req);

            return response;
        }

        private static async Task PutTimesheet(PutStandardTimesheet2Request timesheetRequest)
        {
            AppRequest req = new AppRequest();
            req.serviceURL = PutStandardTimesheet2Request.ServiceUrl;
            req.Input = JObject.FromObject(timesheetRequest);

            await GetServerData(req);

        }


        private static async Task<JToken> GetServerData(AppRequest appRequest)
        {

            JObject jObject;
            try
            {
                using (var client = new HttpClient(new HttpClientHandler(){ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true }))
                {
                    using (var request = new HttpRequestMessage(HttpMethod.Post, $"{Credentials.RepliConnectURL}/api/values/x"))
                    {
                        using (var httpContent = CreateHttpContent(appRequest))
                        {
                            request.Content = httpContent;
                            using (var response = await client
                                .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, CancellationToken.None)
                                .ConfigureAwait(false))
                            {
                                response.EnsureSuccessStatusCode();
                                using (var receivedStream = await response.Content.ReadAsStreamAsync())
                                {
                                    using (StreamReader reader = new StreamReader(receivedStream))
                                    {
                                        var line =  await reader.ReadLineAsync();
                                        jObject = JObject.Parse(line); // Convert the response to JSON Object
                                    }
                                    return jObject;
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
#warning Do something with error messages
                Dictionary<string, string> errorMessages;

                if (e.GetType() == typeof(WebException)) // Check if the exception is a Web Exception(returned by the HTTP request)
                {
                    Dictionary<string, string> error = new Dictionary<string, string>(); // Create a Dictionary to hold the Error Details
                    WebException eWebException = (WebException)e;
                    JObject errorJObject = GetResponseErrorObject((HttpWebResponse)eWebException.Response, out error); // Get the formatted Error Details

                    if (!error.Any())
                    {
                        errorMessages = new Dictionary<string, string>()
                        {
                            {"Exception", e.Message},
                            {"Details", errorJObject["error"]["reason"].ToString()},
                            {"CorelationID", eWebException.Response.Headers["X-Execution-Correlation-Id"]}
                        };
                    }
                    else
                    {
                        errorMessages = error;
                    }
                    return errorJObject;
                }
                else
                {
                    errorMessages = new Dictionary<string, string>()
                    {
                        {"Exception", e.Message},
                        {"Details", string.Empty},
                        {"CorelationID", string.Empty}
                    };
                    return null;
                }
            }
        }


//        private static JToken GetServerData(AppRequest appRequest, string requestMethod = "POST")
//        {
//            //ignore http errors
//#warning This is due to self signed cert, should fix
//            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;


//            WebRequest req = WebRequest.Create($"{Credentials.RepliConnectURL}/api/values/x");
//            req.ContentType = "application/json";
//            req.Method = requestMethod;
//            using (var streamWriter = new StreamWriter(req.GetRequestStream()))
//            {
//                string json = JsonConvert.SerializeObject(appRequest);

//                streamWriter.Write(json);
//                streamWriter.Flush();
//                streamWriter.Close();
//            }

//            try
//            {
//                var httpResponse = (HttpWebResponse)req.GetResponse();
//                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
//                {
//                    return streamReader.ReadToEnd();
//                }
//            }
//            catch (Exception e)
//            {
//                return e.Message;
//            }


//        }


        #endregion

        #region Helpers

        public static void SerializeJsonIntoStream(object value, Stream stream)
        {
            using (var sw = new StreamWriter(stream, new UTF8Encoding(false), 1024, true))
            using (var jtw = new JsonTextWriter(sw) { Formatting = Formatting.None })
            {
                var js = new JsonSerializer();
                js.Serialize(jtw, value);
                jtw.Flush();
            }
        }


        private static HttpContent CreateHttpContent(object content)
        {
            HttpContent httpContent = null;

            if (content != null)
            {
                var ms = new MemoryStream();
                SerializeJsonIntoStream(content, ms);
                ms.Seek(0, SeekOrigin.Begin);
                httpContent = new StreamContent(ms);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            return httpContent;
        }

        private static JObject GetResponseErrorObject(HttpWebResponse errorResponse, out Dictionary<string, string> error)
        {
            try
            {
                JObject jObject;
                using (Stream receivedstream = errorResponse.GetResponseStream())
                using (StreamReader reader = new System.IO.StreamReader(receivedstream))
                {
                    jObject = JObject.Parse(reader.ReadLine());
                }
                error = new Dictionary<string, string>();
                return jObject;
            }
            catch (Exception e)
            {
                error = new Dictionary<string, string>()
                {
                    {"Exception", e.Message},
                    {"Details", e.Message},
                    {"CorelationID", string.Empty}
                };

                return null;
            }

        }

        #endregion

    }
}
