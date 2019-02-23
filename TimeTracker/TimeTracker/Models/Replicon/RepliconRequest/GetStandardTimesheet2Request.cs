using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTracker.Models.Replicon.RepliconRequest
{
    public class GetStandardTimesheet2Request
    {
        public string timesheetUri { get; set; }

        public static string ServiceUrl => "/services/TimesheetService1.svc/GetStandardTimesheet2";
    }
}
