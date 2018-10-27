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
        public string self { get; set; }
        public string key { get; set; }
        [Ignore]
        public Fields fields { get; set; }

        public string Summary
        {
            get => fields.summary;
            set => fields.summary = value;
        }

        public string RepliconID {
            get => fields.customfield_10571;
            set => fields.customfield_10571 = value;
        }
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
