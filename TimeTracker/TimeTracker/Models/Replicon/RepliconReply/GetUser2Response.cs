using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTracker.Models.Replicon.RepliconReply
{
    public class GetUser2Response
    {
        public UserRootObject d { get; set; } 

    }

    public class UserRootObject
        {
           public string displayText { get; set; }
           public string loginName { get; set; }

           public string slug { get; set; }

           public string uri { get; set; }
        }



}
