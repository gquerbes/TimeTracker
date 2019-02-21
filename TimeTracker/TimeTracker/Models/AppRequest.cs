using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TimeTracker.Models
{
    public class AppRequest 
    {
        public string serviceURL { get; set; }

        public JObject Input { get; set; }
    }
}
