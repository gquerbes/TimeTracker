using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTracker.Models.Replicon.RepliconReply
{
    public class GetPageOfProjectsFilteredByClientAndTextSearchResponse
    {
        public List<ProjectRootObject> d { get; set; }
    }

    public class Client
    {
        public string displayText { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public string uri { get; set; }
    }

   

    public class DateRangeWhereTimeAllocationIsAllowed
    {
        public EndDate endDate { get; set; }
        public StartDate startDate { get; set; }
    }

    

    public class ProjectRootObject
    {
        public Client client { get; set; }
        public List<Client> clients { get; set; }
        public DateRangeWhereTimeAllocationIsAllowed dateRangeWhereTimeAllocationIsAllowed { get; set; }
        public bool hasTasksAvailableForTimeAllocation { get; set; }
        public bool isTimeAllocationAllowed { get; set; }
        public object program { get; set; }
        public Project project { get; set; }
    }

  

}
