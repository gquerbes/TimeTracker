﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SQLite;
using TimeTracker.Models.Replicon.RepliconReply;

namespace TimeTracker.Models.Replicon.RepliconRequest
{
    public class PutStandardTimesheet2Request
    {
        [JsonIgnore]
        public static string ServiceUrl => "/services/TimesheetService1.svc/PutStandardTimesheet2";

        public Timesheet timesheet { get; set; } = new Timesheet();
    }


    public class Target
    {
        public string uri { get; set; }
        public object user { get; set; }
        public object date { get; set; }
    }

    public class Task
    {
        public string uri { get; set; }
        public object name { get; set; }
        public object parent { get; set; }
        public object parameterCorrelationId { get; set; }
    }

   
   

    public class Timesheet
    {
        public Target target { get; set; } = new Target();
        public List<CustomField> customFields { get; set; }
        public List<Row> rows { get; set; } = new List<Row>();
        public string noticeExplicitlyAccepted { get; set; }
        public object bankedTime { get; set; }
    }

  
}
