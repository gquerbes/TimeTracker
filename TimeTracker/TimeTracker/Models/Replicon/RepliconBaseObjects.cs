using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTracker.Models.Replicon.RepliconReply
{
    public class Project
    {
        public string displayText { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public string uri { get; set; }
    }

    public class EndDate
    {
        public int day { get; set; }
        public int month { get; set; }
        public int year { get; set; }
    }

    public class StartDate
    {
        public int day { get; set; }
        public int month { get; set; }
        public int year { get; set; }
    }

    public class TaskBasic
    {
        public string displayText { get; set; }
        public string uri { get; set; }
    }
  

    public class CustomField2
    {
        public string displayText { get; set; }
        public string groupUri { get; set; }
        public string name { get; set; }
        public string uri { get; set; }
    }

    public class Date
    {
        public int day { get; set; }
        public int month { get; set; }
        public int year { get; set; }
    }

    public class CustomFieldType
    {
        public object displayText { get; set; }
        public string uri { get; set; }
    }

    public class CustomField
    {
        public CustomField2 customField { get; set; }
        public CustomFieldType customFieldType { get; set; }
        public string text { get; set; }
    }

    public class Row
    {
        public object activity { get; set; }
        public BillingRate billingRate { get; set; }
        public List<Cell> cells { get; set; }
        public List<CustomFieldValue2> customFieldValues { get; set; }
        public Project project { get; set; }
        public TaskBasic TaskBasic { get; set; }
        public string uri { get; set; }
    }

    public class CustomFieldValue
    {
        public CustomField3 customField { get; set; }
        public CustomFieldType2 customFieldType { get; set; }
        public string text { get; set; }
    }

    public class Cell
    {
        public string comments { get; set; }
        public List<CustomFieldValue> customFieldValues { get; set; }
        public Date date { get; set; }
        public Duration duration { get; set; }
    }

    public class Duration
    {
        public string hours { get; set; }
        public string minutes { get; set; }
        public string seconds { get; set; }
        public string milliseconds { get; set; }
        public string microseconds { get; set; }
    }
}
