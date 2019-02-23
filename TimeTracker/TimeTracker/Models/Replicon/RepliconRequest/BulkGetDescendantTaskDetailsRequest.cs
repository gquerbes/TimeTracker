using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TimeTracker.Models.Replicon.RepliconRequest
{
    public class BulkGetDescendantTaskDetailsRequest
    {
        public List<string> parentUris { get; set; } = new List<string>();

        [JsonIgnore] public static string ServiceURL =>  "/services/TaskService1.svc/BulkGetDescendantTaskDetails";
    }
}
