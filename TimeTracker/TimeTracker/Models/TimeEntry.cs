using System;
using System.Collections.Generic;
using System.Text;
using TimeTracker.Database;

namespace TimeTracker.Models
{
    public class TimeEntry : DataObj
    {
        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; } = DateTime.MinValue;

        public string Comments { get; set; }

        public Guid TicketID { get; set; }

    }
}
