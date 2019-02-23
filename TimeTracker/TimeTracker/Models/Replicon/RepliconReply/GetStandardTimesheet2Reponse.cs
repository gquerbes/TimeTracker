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

    public class CustomFieldValue
    {
        public CustomField3 customField { get; set; }
        public CustomFieldType2 customFieldType { get; set; }
        public string text { get; set; }
    }

    public class Date
    {
        public int day { get; set; }
        public int month { get; set; }
        public int year { get; set; }
    }

    public class Duration
    {
        public int hours { get; set; }
        public int microseconds { get; set; }
        public int milliseconds { get; set; }
        public int minutes { get; set; }
        public int seconds { get; set; }
    }

    public class Cell
    {
        public string comments { get; set; }
        public List<CustomFieldValue> customFieldValues { get; set; }
        public Date date { get; set; }
        public Duration duration { get; set; }
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
