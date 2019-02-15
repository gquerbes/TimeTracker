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
using TimeTracker.Interfaces;
using TimeTracker.Models;
using TimeTracker.ViewModels;
using Xamarin.Forms;

namespace TimeTracker
{
    public class TimeEntryViewModel :  ITimeEntryListElement, INotifyPropertyChanged
    {

        public TimeEntryViewModel(TimeEntry Entry = null)
        {
            //set current time entry if one being passed in else, create new entry
            TimeEntry = Entry ?? new TimeEntry();
        }

        public TimeEntryParent parent { get; set; }
   

        #region Properties



        public TimeEntry TimeEntry{get; set; }

        public DateTime StartTime
        {
            get => TimeEntry.StartDateTime;
            set => TimeEntry.StartDateTime = value;
        }

        public DateTime EndTime
        {
            get => TimeEntry.EndDateTime;
            set => TimeEntry.EndDateTime = value;
        }

        private Ticket _ticket;
        public Ticket Ticket
        {
            get
            {
                if (_ticket == null && !string.IsNullOrEmpty(TimeEntry.TicketRepliconID) )
                {
                    _ticket = App.Database.FindTicketByRepliconID(TimeEntry.TicketRepliconID);
                }

                return _ticket;
            }
            set
            {
                _ticket = value;
                TimeEntry.TicketRepliconID = value?.repliconID;
                OnPropertyChanged(nameof(SelectedTicketLabel));
            }
        }

        /// <summary>
        /// Returns TRUE if end time has not been set yet
        /// </summary>
        public bool IsRunning => !StartTime.Equals(DateTime.MinValue) && EndTime.Equals(DateTime.MinValue);

        public TimeSpan RunTime
        {
            get
            {
                if (IsRunning)
                {
                    return DateTime.Now - TimeEntry.StartDateTime;
                }

                return TimeEntry.EndDateTime - TimeEntry.StartDateTime;
            }
        }

        public string Comments
        {
            get => TimeEntry?.Comments;
            set
            {
                if (value !=null && !value.Equals(Comments))
                {
                    TimeEntry.Comments = value;
                    OnPropertyChanged();
                }
            }
        }

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
                if (IsRunning)
                {
                    return  ((Color) Application.Current.Resources["AbasRed"]);
                }
                    return ((Color)Application.Current.Resources["AbasLimeGreen"]);
            }
        }


        public string TimerButtonText
        {
            get
            {
                if (IsRunning)
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
            if (!IsRunning)
            {
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
            if (IsRunning)
            {
                EndTime = DateTime.Now;
                Save();
            }
            else
            {
                throw new Exception("Stopwatch was not running!");
            }
            OnPropertyChanged(nameof(TimerButtonColor));
            OnPropertyChanged(nameof(TimerButtonText));
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
