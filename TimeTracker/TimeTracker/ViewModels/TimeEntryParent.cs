using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using TimeTracker.Annotations;
using TimeTracker.Database;
using TimeTracker.Helpers;
using TimeTracker.Interfaces;
using TimeTracker.Models;
using TimeTracker.Models.Replicon.RepliconReply;

namespace TimeTracker.ViewModels
{
    public class TimeEntryParent : ITimeEntryListElement, INotifyPropertyChanged
    {
        /// <summary>
        /// Holds list of entries for a specific ordertime
        /// </summary>
        public List<TimeEntryViewModel> Entries { get; set; } = new List<TimeEntryViewModel>();

        public string RepliconTicketID { get; set; }

        public DateTime Date { get; set; }

        public bool BillCustomer => Entries.FirstOrDefault() != null ? Entries.FirstOrDefault().BillCustomer : false;

        public bool ExistsOnTimeSheet { get; set; }

        private RepliconTask _ticket;
        public RepliconTask Ticket
        {
            get
            {
                if (_ticket == null && !string.IsNullOrEmpty(RepliconTicketID))
                {
                    _ticket = App.Database.FindRepliconTaskByUri(RepliconTicketID);
                }
                return _ticket;
            }
            set
            {
            
               _ticket = value;

                if (_ticket != null)
                {
                    foreach (var timeEntryViewModel in Entries)
                    {
                        timeEntryViewModel.TimeEntry.TicketURI = value?.uri;  
                    }
                }
                OnPropertyChanged(nameof(SelectedTicketLabel));

            }
        }

        public string Comments
        {
            get
            {
                var text = "";
                foreach (var timeEntryViewModel in Entries)
                {
                    if (!string.IsNullOrEmpty(timeEntryViewModel.Comments))
                    {
                        text += $"{timeEntryViewModel.Comments} ";
                    }
                }

                if (string.IsNullOrEmpty(text))
                {
                    text = "[No Comments]";
                }
                return text;
            }
        }

        public string InternalComments
        {
            get
            {
                var text = "";
                foreach (var timeEntryViewModel in Entries)
                {
                    if (!string.IsNullOrEmpty(timeEntryViewModel.InternalComments))
                    {
                        text += $"{timeEntryViewModel.InternalComments} ";
                    }
                }

                return text;
            }
        }

        public string SelectedTicketLabel => Entries.FirstOrDefault()?.Ticket != null ? Entries.FirstOrDefault()?.SelectedTicketLabel : string.Empty;


        public TimeSpan RoundedTotalTime => TotalTime.RoundToNearestMinutes(15);
        

        public TimeSpan TotalTime
        {
            get
            {
                TimeSpan totalTime = new TimeSpan();
                foreach (var timeEntryViewModel in Entries)
                {
                    totalTime += timeEntryViewModel.RunTime;
                }

                return totalTime;
            }
        }
        public string RunTimeText
        {
            get
            {
                var totalTime = TotalTime;

                return $"{totalTime.Hours}:{totalTime.Minutes.NormalizeIntForTime()}";
            }

        }


        public string RoundedRunTimeText
        {
            get
            {
                var RoundedTime = RoundedTotalTime;
                return $"({(RoundedTime.TotalHours).ToString(CultureInfo.CurrentCulture)})";
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
