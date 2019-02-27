using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTracker.Models.Replicon.RepliconReply
{
    public class GenerateReportResponse
    {
        public ReportResponseRootObject D { get; set; }
    }

    public class ReportResponseRootObject
    {
        public object error { get; set; }
        public List<object> filterValues { get; set; }
        public string payload { get; set; }
        public string reportUri { get; set; }
    }
}
