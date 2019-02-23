using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTracker.Models.Replicon.RepliconReply
{

    public class GetStandardTimesheet2Reply
    {
        public StandardTimesheet2RootObject d { get; set; }
    }

    public class DateRange
    {
        public EndDate endDate { get; set; }
        public StartDate startDate { get; set; }
    }

    public class DueDate
    {
        public int day { get; set; }
        public int month { get; set; }
        public int year { get; set; }
    }

    public class Owner
    {
        public string displayText { get; set; }
        public string loginName { get; set; }
        public string slug { get; set; }
        public string uri { get; set; }
    }

    public class BillingRate
    {
        public string displayText { get; set; }
        public string name { get; set; }
        public string uri { get; set; }
    }

    public class CustomField3
    {
        public string displayText { get; set; }
        public string groupUri { get; set; }
        public string name { get; set; }
        public string uri { get; set; }
    }

    public class CustomFieldType2
    {
        public object displayText { get; set; }
        public string uri { get; set; }
    }

   

  


    public class CustomField4
    {
        public string displayText { get; set; }
        public string groupUri { get; set; }
        public string name { get; set; }
        public string uri { get; set; }
    }

    public class CustomFieldType3
    {
        public object displayText { get; set; }
        public string uri { get; set; }
    }

    public class CustomFieldValue2
    {
        public CustomField4 customField { get; set; }
        public CustomFieldType3 customFieldType { get; set; }
        public string text { get; set; }
    }

  

    

    public class TimesheetNotice
    {
        public string description { get; set; }
        public List<object> timesheetNoticeOptionUris { get; set; }
        public string title { get; set; }
    }

    public class StandardTimesheet2RootObject
    {
        public List<CustomField> customFields { get; set; }
        public DateRange dateRange { get; set; }
        public DueDate dueDate { get; set; }
        public bool noticeExplicitlyAccepted { get; set; }
        public Owner owner { get; set; }
        public List<Row> rows { get; set; }
        public string slug { get; set; }
        public TimesheetNotice timesheetNotice { get; set; }
        public string uri { get; set; }
    }

  
}
