using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace TimeTracker.Database
{
    public abstract class DataObj
    {
        [PrimaryKey]
        public string Guid { get; set; }

    }
}
