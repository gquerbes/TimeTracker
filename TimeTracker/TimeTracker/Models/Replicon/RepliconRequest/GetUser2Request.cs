using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RepliconIntegrator.Models
{
    
    public class GetUser2Request 
    {
        [JsonIgnore]
        public static string ServiceURL => "/services/UserService1.svc/GetUser2";

        public User user { get; set; } = new User();
    }

    public class User
        {
            public string uri { get; set; }
            public string loginName { get; set; }
            public string parameterCorrelationID { get; set; }
        }

     
}
