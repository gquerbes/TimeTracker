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
            protected set => _currentTimeEntry = value;
        }




        public ObservableCollection<TimeEntryParentObservableCollection> _timeEntriesParents;

        public ObservableCollection<TimeEntryParentObservableCollection> TimeEntriesParents
        {
            get
            {
                if (_timeEntriesParents == null)
                {
                    _timeEntriesParents = new ObservableCollection<TimeEntryParentObservableCollection>();
                }

                if (!_timeEntriesParents.Any(x => x.Date.Equals(DateTime.Today)))
                {
                    _timeEntriesParents.Add(new TimeEntryParentObservableCollection(DateTime.Today));
                }

                return _timeEntriesParents;
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
            //check if valid parent ticket exists and place child inside or make new one and place child inside if does not exist
            foreach (var entryParent in  TimeEntriesParents.First())
            {
                if (entryParent.Ticket != null && entryParent.Ticket.repliconID.Equals(vm.Ticket?.repliconID))
                {
                    var castedParent = (entryParent as TimeEntryParent);
                    castedParent?.Entries.Add(vm);
                    castedParent?.OnPropertyChanged(null);
                    return;
                }
            }
            TimeEntriesParents.First().Add(new TimeEntryParent(){Entries = new List<TimeEntryViewModel>(){vm}});
            
        }

        public void ContinueTimerClicked(TimeEntryViewModel previousTimeEntry)
        {
            TimerClicked();
            CurrentTimeEntry.Comments = previousTimeEntry.Comments;
            CurrentTimeEntry.Ticket = previousTimeEntry.Ticket;
            OnPropertyChanged(null);
        }


    

    public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


}
