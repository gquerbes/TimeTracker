using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using TimeTracker.Interfaces;
using TimeTracker.ViewModels;

namespace TimeTracker.Models
{

    /// <summary>
    /// Observable collection to hold entries for a specified date
    /// </summary>
   public class TimeEntryListElementOverservableCollection : ObservableCollection<ITimeEntryListElement>
   {
       public TimeEntryListElementOverservableCollection(DateTime date)
       {
           Date = date;
       }

      
       public DateTime Date { get; set; }
       public string DateLabel => Date.ToString("D");
   }
}
