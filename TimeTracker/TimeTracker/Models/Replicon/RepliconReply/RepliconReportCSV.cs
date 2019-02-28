using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTracker.Models.Replicon.RepliconReply
{
    public class RepliconReportCSV
    {
        [CsvHelper.Configuration.Attributes.Name("Project Name")]
        public string ProjectName { get; set; }

        [CsvHelper.Configuration.Attributes.Name("Task Name (Full Path)")]
        public string TaskName { get; set; }
        [CsvHelper.Configuration.Attributes.Name("Billing Type")]

        public string BillingType { get; set; }
        [CsvHelper.Configuration.Attributes.Name("Task Code")]

        public string TaskCode { get; set; }
        [CsvHelper.Configuration.Attributes.Name("Task URI")]

        public string TaskURI { get; set; }
        [CsvHelper.Configuration.Attributes.Name("Project URI")]

        public string ProjectURI { get; set; }

    }
}
