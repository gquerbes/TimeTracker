using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TimeTracker.Models.Replicon.RepliconRequest
{
    public class GetTimesheetForDate2Request
    {
        /// <summary>
        /// Service URL of  GetTimesheetForDate2
        /// </summary>
        [JsonIgnore] public static string ServiceURL => "/services/TimesheetService1.svc/GetTimesheetForDate2";

        public string userUri { get; set; }
        public Date date { get; set; } = new Date();
        public string timesheetGetOptionUri { get; set; }


        /// <summary>
        /// Set's date fields as required
        /// </summary>
        /// <param name="date"></param>
        public void SetDate(DateTime date)
        {
            this.date.year = date.Year;
            this.date.month = date.Month;
            this.date.day = date.Day;
        }

       
    }

    public class Date
    {
        public int year { get; set; }
        public int month { get; set; }
        public int day { get; set; }
    }


}
