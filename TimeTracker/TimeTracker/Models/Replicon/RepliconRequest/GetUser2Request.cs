using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TimeTracker.Models.Replicon.RepliconReply;

namespace RepliconIntegrator.Models
{
    
    public class GetUser2Request 
    {
        [JsonIgnore]
        public static string ServiceURL => "/services/UserService1.svc/GetUser2";

        public User user { get; set; } = new User();
    }

    

     
}
