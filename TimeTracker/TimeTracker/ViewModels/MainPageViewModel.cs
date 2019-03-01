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
using System.Threading.Tasks;
using TimeTracker.Annotations;
using TimeTracker.Database;
using TimeTracker.Models;
using TimeTracker.Services;
using TimeTracker.ViewModels;
using Xamarin.Forms;

namespace TimeTracker
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private TimeEntryViewModel _currentTimeEntry;

        public TimeEntryViewModel CurrentTimeEntry
        {
            get
            {
                if (_currentTimeEntry == null)
                {
                    _currentTimeEntry = new TimeEntryViewModel();
                }

                return _currentTimeEntry;
            }
            protected set
            {

                _currentTimeEntry = value;
                OnPropertyChanged(null);
            } 
        }


        public void ContinueRunningTimer(TimeEntryViewModel vm)
        {
            this.CurrentTimeEntry = vm;
        }

        public ObservableCollection<TimeEntryListElementOverservableCollection> _timeEntries;

        public ObservableCollection<TimeEntryListElementOverservableCollection> TimeEntries
        {
            get
            {
                if (_timeEntries == null)
                {
                    _timeEntries = new ObservableCollection<TimeEntryListElementOverservableCollection>();
                }

                if (!_timeEntries.Any(x => x.Date.Equals(DateTime.Today)))
                {
                    _timeEntries.Add(new TimeEntryListElementOverservableCollection(DateTime.Today));
                }

                return _timeEntries;
            }
        }



        /// <summary>
        /// Start or stop current job
        /// </summary>
        public void TimerClicked()
        {
            if (!CurrentTimeEntry.IsRunning)
            {
                CurrentTimeEntry.StartTimer();
            }
            else
            {
                CurrentTimeEntry.StopTimer();
                AddTimeEntryToList(CurrentTimeEntry);
                _currentTimeEntry = new TimeEntryViewModel();
                OnPropertyChanged(nameof(CurrentTimeEntry));
                CurrentTimeEntry.OnPropertyChanged(nameof(TimeEntryViewModel.Comments));
            }
        }


        /// <summary>
        /// Will add time entry to corresponding parent or make new parent if required
        /// </summary>
        /// <param name="vm"></param>
        public void AddTimeEntryToList(TimeEntryViewModel vm)
        {
            //correspodning date collection
            var collection = TimeEntries.FirstOrDefault(x => x.Date.Equals(vm.StartTime.Date));
            //check if valid parent ticket exists and place child inside or make new one and place child inside if does not exist
            foreach (var entryParent in  collection)
            {
                if (entryParent.Ticket != null && entryParent.Ticket.uri.Equals(vm.Ticket?.uri))
                {
                    var castedParent = (entryParent as TimeEntryParent);
                    vm.parent = castedParent;
                    castedParent?.Entries.Add(vm);
                    castedParent?.OnPropertyChanged(null);
                    return;
                }
            }

            //No valid parent yest on list
            //create new parent
            var newParent = new TimeEntryParent();
            newParent.Date = DateTime.Today;
            //add new time entry as the first item in the new parent's Entries list
            newParent.Entries = new List<TimeEntryViewModel>(){vm};
            //set the ticket for the parent
            newParent.Ticket = vm.Ticket;
            //set the parent of the entry to the newly created parent
            vm.parent = newParent;
            
            // add to list
            TimeEntries.First().Add(newParent);
            
        }

        public void ContinueTimerClicked(TimeEntryViewModel previousTimeEntry)
        {
            TimerClicked();
            CurrentTimeEntry.Ticket = previousTimeEntry.Ticket;
            OnPropertyChanged(null);
        }

        public async Task LoadTickets(IProgress<double> progress)
        {
           await TicketLoader.LoadData(progress);
        }

        public void SubmitTimesheet()
        {
            var y = TimeEntries?.FirstOrDefault()?.Cast<TimeEntryParent>().ToList();
            RepliConnect.SubmitTimesheet(y);
        }

        public void ParseDateEntry(string entry)
        {
            var validParse = TimeSpan.TryParse(entry, out TimeSpan result );

            if (validParse)
            {
                var correctedStartTime = DateTime.Now - result;
                CurrentTimeEntry.StartTime = correctedStartTime;
            }
        }

        private double _syncProgress;
        public double SyncProgress
        {
            get => _syncProgress;
            set
            {
                if (!value.Equals(_syncProgress))
                {
                    _syncProgress = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsSyncBarVisible));
                }
            }
        }

        public bool IsSyncBarVisible => !SyncProgress.Equals(1) && !SyncProgress.Equals(0);

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


}
