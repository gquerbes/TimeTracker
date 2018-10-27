using System;
using System.Collections.Generic;
using System.Text;
using TimeTracker.Database;

namespace TimeTracker.Models
{
    public class TimeEntry : DataObj
    {
        public DateTime StartDate { get; set; }

        public TimeSpan RunTime { get; set; }

        public string Comments { get; set; }

    }
}
