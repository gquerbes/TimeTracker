using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using TimeTracker.Database;

namespace TimeTracker.Database
{
    public class JiraResponse
    {
        public int startAt { get; set; }
        public int total { get; set; }
        public List<Ticket> issues { get; set; }
    }

    public class Ticket : DataObj
    {
        public string id { get; set; }
        /// <summary>
        /// URL to ticket
        /// </summary>
        public string self { get; set; }
        /// <summary>
        /// Ticket abbreviation i.e. ABC-123
        /// </summary>
        public string key { get; set; }

        /// <summary>
        /// Ticket Summary
        /// </summary>
        public string Summary { get; set; }

        public string repliconID { get; set; }

        [Ignore]
        public Fields fields { get; set; }
    }

    public class Fields : DataObj
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
