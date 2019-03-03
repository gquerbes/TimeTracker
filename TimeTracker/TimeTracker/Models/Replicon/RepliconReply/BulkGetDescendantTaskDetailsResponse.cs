using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SQLite;
using TimeTracker.Database;

namespace TimeTracker.Models.Replicon.RepliconReply
{
    public class BulkGetDescendantTaskDetailsResponse
    {

        public List<TaskRootObject> d { get; set; }
    }

   

        public class EstimatedHours
        {
            public int hours { get; set; }
            public int minutes { get; set; }
            public int seconds { get; set; }
        }

        public class TimeAndExpenseEntryType
        {
            public string displayText { get; set; }
            public string uri { get; set; }
        }

        public class TimeEntryDateRange
        {
            public object endDate { get; set; }
            public object startDate { get; set; }
        }

    public class  RepliconTask : DataObj
    {
        [Ignore]
        public string code { get; set; }
        [Ignore]
        public object costType { get; set; }
        [Ignore]
        public List<CustomField> customFields { get; set; }
        public string description { get; set; }
        public string displayText { get; set; }
        [Ignore]
        public object estimatedCost { get; set; }
        [Ignore]
        public EstimatedHours estimatedHours { get; set; }
        [Ignore]
        public bool isClosed { get; set; }
        [Ignore]
        public bool isClosedByInheritance { get; set; }
        [Ignore]
        public bool isTimeEntryAllowed { get; set; }
        public string name { get; set; }
        [Ignore]
        public object parent { get; set; }
        [IgnoreDataMember]
        public int percentCompleted { get; set; }
        [Ignore]
        public object slug { get; set; }
        [Ignore]
        public TimeAndExpenseEntryType timeAndExpenseEntryType { get; set; }
        [Ignore]
        public TimeEntryDateRange timeEntryDateRange { get; set; }
        public string uri { get; set; }

        public string ProjectURI { get; set; }


        [JsonIgnore]
        public bool IsBillable { get; set; }
    }

    public class ChildTask
        {
            public List<RepliconTask> tasks { get; set; }
            public RepliconTask Task { get; set; }
        }

    public class TaskRootObject
        {
            public List<ChildTask> childTasks { get; set; }
            public string parent { get; set; }
        }

      
}
