using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace TimeTracker.Models
{

    /// <summary>
    /// Observable collection to hold entries for a specified date
    /// </summary>
   public class TimeEntryObservableCollection : ObservableCollection<TimeEntryViewModel>
   {
       public TimeEntryObservableCollection(DateTime date)
       {
           Date = date;
       }

      
       public DateTime Date { get; set; }
       public string DateLabel => Date.ToString("D");
   }
}
