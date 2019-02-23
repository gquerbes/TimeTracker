using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTracker.Models.Replicon.RepliconReply
{
    public class RepliconReponseBase
    {

    }
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

    public class Task
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

}
