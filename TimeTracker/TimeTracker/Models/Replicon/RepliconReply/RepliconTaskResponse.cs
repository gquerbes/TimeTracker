using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RepliconIntegrator.Models.RepliconReply
{
    public class RepliconTaskResponse
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

        //public class Task 
        //{
        //    public string code { get; set; }
        //    public object costType { get; set; }
        //    public List<CustomField> customFields { get; set; }
        //    public string description { get; set; }
        //    public string displayText { get; set; }
        //    public object estimatedCost { get; set; }
        //    public EstimatedHours estimatedHours { get; set; }
        //    public bool isClosed { get; set; }
        //    public bool isClosedByInheritance { get; set; }
        //    public bool isTimeEntryAllowed { get; set; }
        //    public string name { get; set; }
        //    public object parent { get; set; }
        //    public int percentCompleted { get; set; }
        //    public object slug { get; set; }
        //    public TimeAndExpenseEntryType timeAndExpenseEntryType { get; set; }
        //    public TimeEntryDateRange timeEntryDateRange { get; set; }
        //}

        public class ChildTask
        {
            public List<Task> childTasks { get; set; }
            public Task task { get; set; }
        }

    public class TaskRootObject
        {
            public List<ChildTask> childTasks { get; set; }
            public string parent { get; set; }
        }

      
}
