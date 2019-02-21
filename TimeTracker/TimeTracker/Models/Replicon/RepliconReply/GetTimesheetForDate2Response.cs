using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTracker.Models.Replicon.RepliconReply
{
    class GetTimesheetForDate2Response
    {
        public TimeSheetRootObject d { get; set; }
    }

    public class Timesheet
    {
        public string displayText { get; set; }
        public string slug { get; set; }
        public string uri { get; set; }
    }

    public class TimeSheetRootObject
    {
        public Timesheet timesheet { get; set; }
        public string timesheetFormat { get; set; }
    }

}
