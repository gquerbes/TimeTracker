using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TimeTracker.Models.Replicon.RepliconRequest
{
    public class GetPageOfProjectsFilteredByClientAndTextSearchRequest
    {
        public int page { get; set; } = 1;
        public int pageSize { get; set; } = 1000;
        public string timesheetUri { get; set; }
        public string clientUri { get; set; }
        public string clientNullFilterBehaviorUri { get; set; }


        [JsonIgnore]
        public static string ServiceURL =>
            "/services/TaskSelectorService1.svc/GetPageOfProjectsFilteredByClientAndTextSearch";

    }
}
