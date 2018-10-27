using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTracker.Database
{
    public class JiraResponse
    {
        public int startAt { get; set; }
        public int total { get; set; }
        public List<Issue> issues { get; set; }
    }

    public class Issue
    {
        public string id { get; set; }
        public string self { get; set; }
        public string key { get; set; }
        public Fields fields { get; set; }
    }

    public class Fields
    {
        /// <summary>
        /// Ticket Summary
        /// </summary>
        public string summary { get; set; }

        /// <summary>
        /// RepliconIDNumber
        /// </summary>
        public string customfield_10571 { get; set; }
    }

}
