using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using TimeTracker.Database;

namespace TimeTracker.Models
{
    public class TimeEntry : DataObj
    {
        public DateTime StartDateTime { get; set; } = DateTime.MinValue;

        public DateTime EndDateTime { get; set; } = DateTime.MinValue;

        public string Comments { get; set; }

        public string InternalComments { get; set; }

        public string TicketURI { get; set; }

        public bool BillCustomer { get; set; }



    }
}
