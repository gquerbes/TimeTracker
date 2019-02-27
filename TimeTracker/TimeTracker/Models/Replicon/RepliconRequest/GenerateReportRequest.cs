using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace TimeTracker.Models.Replicon.RepliconRequest
{
    public class GenerateReportRequest
    {
        [JsonIgnore] public static string ServiceURL => "/services/ReportService1.svc/GenerateReport";

        public string reportUri { get; set; }
        public List<object> filterValues { get; set; } = new List<object>();

        public string outputFormatUri { get; set; }
    }
}
