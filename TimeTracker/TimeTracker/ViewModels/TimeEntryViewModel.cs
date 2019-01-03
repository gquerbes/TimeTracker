using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using TimeTracker.Annotations;
using TimeTracker.Database;
using TimeTracker.Models;
using Xamarin.Forms;

namespace TimeTracker
{
    public class TimeEntryViewModel :  INotifyPropertyChanged
    {

        public TimeEntryViewModel(TimeEntry Entry)
        {
            TimeEntry = Entry;
            Stopwatch = new Stopwatch();
        }

        public TimeEntryViewModel()
        {
            TimeEntry = new TimeEntry();
            Stopwatch = new Stopwatch();
        }

        #region Properties



        protected TimeEntry TimeEntry{get; set; }

        public DateTime StartTime => TimeEntry.StartDateTime;

        private Ticket _ticket;
        public Ticket Ticket
        {
            get
            {
                if (_ticket == null &&  !string.IsNullOrEmpty(TimeEntry.TicketRepliconID) )
                {
                    _ticket = App.Database.FindTicketByRepliconID(TimeEntry.TicketRepliconID);
                }

                return _ticket;
            }
            set
            {
                _ticket = value;
                TimeEntry.TicketRepliconID = value.repliconID;
                OnPropertyChanged(nameof(SelectedTicketLabel));
            }
        }


        public TimeSpan RunTime
        {
            get
            {
                if (TimeEntry.EndDateTime.Equals(DateTime.MinValue))
                {
                    return DateTime.Now - TimeEntry.StartDateTime;
                }

                return TimeEntry.EndDateTime - TimeEntry.StartDateTime;
            }
        }

        public string Comments
        {
            get => TimeEntry?.Comments;
            set => TimeEntry.Comments = value;
        }

        public Stopwatch Stopwatch;
        public string Guid { get; set; }
        #endregion


        #region BindedProperties

        public string SelectedTicketLabel
        {
            get => $"{Ticket?.key} : {Ticket?.Summary}";
        }

        public string RunTimeText
        {
            get { return $"{RunTime.Hours}:{RunTime.Minutes}:{RunTime.Seconds}"; }
        }

        public Color TimerButtonColor
        {
            get
            {
                if (Stopwatch.IsRunning)
                {
                    return Color.FromHex("#97d700");
                }

                return Color.FromHex("#E40046");
            }
        }


        public string TimerButtonText
        {
            get
            {
                if (Stopwatch.IsRunning)
                {
                    return "Stop";
                }

                return "Start";
            }
        }
        #endregion

        #region Methods

        public void StartTimer()
        {
            if (!Stopwatch.IsRunning)
            {
                Stopwatch.Start();
                TimeEntry.StartDateTime = DateTime.Now;
            }
            else
            {
                throw new Exception("Stopwatch already running!");
            }

            OnPropertyChanged(nameof(TimerButtonColor));
            OnPropertyChanged(nameof(TimerButtonText));
        }

        public void StopTimer()
        {
            if (Stopwatch.IsRunning)
            {
                Stopwatch.Stop();
                Save();
            }
            else
            {
                throw new Exception("Stopwatch was not running!");
            }
        }


        public  void Save()
        {
             App.Database.SaveItem(TimeEntry);
        }

        public  void Delete()
        {
             App.Database.DeleteItem(TimeEntry);
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
