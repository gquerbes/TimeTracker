using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TimeTracker.Models.Replicon
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
