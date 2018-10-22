using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using TimeTracker.Annotations;
using TimeTracker.Models;
using Xamarin.Forms;

namespace TimeTracker
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private TimeEntry _currentTimeEntry;
        public TimeEntry CurrentTimeEntry
        {
            get
            {
                if (_currentTimeEntry == null)
                {
                    _currentTimeEntry = new TimeEntry();
                }

                return _currentTimeEntry;
            }
            protected set => _currentTimeEntry = value;
        }
  

        

        public ObservableCollection<TimeEntryObservableCollection> _timeEntries;

        public ObservableCollection<TimeEntryObservableCollection> TimeEntries
        {
            get
            {
                if (_timeEntries == null)
                {
                    _timeEntries = new ObservableCollection<TimeEntryObservableCollection>();
                }
                if(!_timeEntries.Any(x => x.Date.Equals(DateTime.Today)))
                {
                    _timeEntries.Add(new TimeEntryObservableCollection(DateTime.Today));
                }

                return _timeEntries;
            }
        }



        public void TimerClicked()
        {
            if (!CurrentTimeEntry.Stopwatch.IsRunning)
            {
                CurrentTimeEntry.StartTimer();
            }
            else
            {
                CurrentTimeEntry.StopTimer();
                TimeEntries.First().Add(CurrentTimeEntry);
                _currentTimeEntry = new TimeEntry();
                OnPropertyChanged(nameof(CurrentTimeEntry));
                CurrentTimeEntry.OnPropertyChanged(nameof(TimeEntry.Comments));
            }
        }


        public void ContinueTimerClicked(TimeEntry previousTimeEntry)
        {

        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


}
