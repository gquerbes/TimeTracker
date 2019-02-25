using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using TimeTracker.Annotations;
using TimeTracker.Database;
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

        public bool IsBillable { get; set; }

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
                if (value != null && !value.Equals(_ticket))
                {
                    _ticket = value;
                    foreach (var timeEntryViewModel in Entries)
                    {
                        timeEntryViewModel.TimeEntry.TicketURI = value?.uri;  
                    }
                    OnPropertyChanged(nameof(SelectedTicketLabel));
                }
            }
        }

        public string Comments
        {
            get
            {
                var text = "";
                foreach (var timeEntryViewModel in Entries)
                {
                    text += $"{timeEntryViewModel.Comments}, ";
                }

                return text;
            }
        }

        public string SelectedTicketLabel => Entries.FirstOrDefault()?.SelectedTicketLabel;


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

                return $"{totalTime.Hours}:{totalTime.Minutes}:{totalTime.Seconds}";
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
