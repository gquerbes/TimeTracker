using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTracker.Models.Replicon.RepliconRequest
{
    public class PutStandardTimesheet2Request
    {

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


    public class Duration
    {
        public string hours { get; set; }
        public string minutes { get; set; }
        public string seconds { get; set; }
        public string milliseconds { get; set; }
        public string microseconds { get; set; }
    }

    public class Cell
    {
        public Date date { get; set; }
        public Duration duration { get; set; }
        public string comments { get; set; }
        public List<object> customFieldValues { get; set; }
    }

    public class Row
    {
        public object target { get; set; }
        public object project { get; set; }
        public Task task { get; set; }
        public object billingRate { get; set; }
        public object activity { get; set; }
        public List<object> customFieldValues { get; set; }
        public List<Cell> cells { get; set; }
    }

    public class Timesheet
    {
        public Target target { get; set; } = new Target();
        public List<object> customFields { get; set; }
        public List<Row> rows { get; set; } = new List<Row>();
        public string noticeExplicitlyAccepted { get; set; }
        public object bankedTime { get; set; }
    }

  
}
