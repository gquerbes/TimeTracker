using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTracker
{
   public class Ticket
   {
       public List<TimeEntry> Entries;
        private string RepliconID { get; set; }

       private string Description { get; set; }

       private string Key { get; set; }
   }
}
