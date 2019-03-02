using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;
using TimeTracker.Helpers;
using TimeTracker.Interfaces;
using TimeTracker.ViewModels;
using TimeTracker.Views;

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


       protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
       {
           base.OnCollectionChanged(e);
           OnPropertyChanged(new PropertyChangedEventArgs(nameof(TotalTimeLabel)));

        }

        public DateTime Date { get; set; }
       public string DateLabel => Date.ToString("D");

   
       public string TotalTimeLabel
       {
           get
           {
               var times = TotalTime;
               var billable= times.Item1;
               var nonbillable = times.Item2;
                return  $"Billed: {billable.TotalHours}\nNon-Bill: {nonbillable.TotalHours}";
           }
       }
       private Tuple<TimeSpan, TimeSpan> TotalTime
       {
           get
           {
               long totalNonBillTime = 0;
               long totalBillTime = 0;
               foreach (var timeEntryListElement in this.Items)
               {
                   if (timeEntryListElement is TimeEntryParent parent)
                   {
                       if (parent.BillCustomer)
                       {
                           totalBillTime += parent.RoundedTotalTime.Ticks;
                       }
                       else
                       {
                           totalNonBillTime += parent.RoundedTotalTime.Ticks;
                       }
                   }
               }
               return new Tuple<TimeSpan, TimeSpan>(new TimeSpan(totalBillTime), new TimeSpan(totalNonBillTime));
           }
        }



   }
}
