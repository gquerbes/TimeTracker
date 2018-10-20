using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTracker
{
    public class Entry
    {
        #region Properties
        public TimeSpan RunTime { get; set; }
        private string TicketID { get; set; }
        public string Comments { get; set; }
        #endregion


        #region BindedProperties

        public string RunTimeText
        {
            get { return $"{RunTime.Hours}:{RunTime.Minutes}:{RunTime.Seconds}"; }
        }
        

        #endregion
    }
}
