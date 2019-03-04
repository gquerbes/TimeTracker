using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TimeTracker.Helpers;
using TimeTracker.Interfaces;
using TimeTracker.Models;
using TimeTracker.ViewModels;
using TimeTracker.Views;
using Xamarin.Forms;

namespace TimeTracker
{
    public partial class MainPage : ContentPage
    {
        private MainPageViewModel _vm;
        private SynchronizationContext _ui;
        public MainPage()
        {
            _vm = new MainPageViewModel();
            this.BindingContext = _vm;

            InitializeComponent();
            LoadTickets();
            EntryListView.OnContinueEntry += OnContinueEntry;
            EntryListView.OnExpandCollapseParent += OnExpandCollapseParent;
            _ui = SynchronizationContext.Current;

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (_vm.CurrentTimeEntry?.Ticket == null)
            {
                AutoCompleteList.SearchBar.Focus();
            }
            else
            {
                CommentsTextEditor.Focus();
            }
        }

        private void OnExpandCollapseParent(TimeEntryParent timeentryparent)
        {
            //sub list that contains all TimeEntryParents for specified date of the selected timeEntryParent
            var correspondingDateList =
                _vm.TimeEntries.FirstOrDefault(x => x.Date.Date.Equals(timeentryparent.Date.Date));

            if(correspondingDateList == null)return;


            if (correspondingDateList.Contains(timeentryparent.Entries.FirstOrDefault()))
            {
                foreach (var childEntry in timeentryparent.Entries)
                {
                    correspondingDateList.Remove(childEntry);
                }
                return;
            }

            int index = -1;
            foreach (var timeEntryVM in timeentryparent.Entries)
            {
                if (index.Equals(-1))
                {
                    index = correspondingDateList.IndexOf(timeentryparent);
                }
               correspondingDateList.Insert(index+1, timeEntryVM);

            }
        }


        /// <summary>
        /// Load data from the DB and add to table
        /// </summary>
        private void LoadTickets()
        {
            //Query entries
            var entries =  App.Database.GetItems<TimeEntry>();
            foreach (var entry in entries)
            {
                if (entry.EndDateTime.Equals(DateTime.MinValue))
                {
                    _vm.ContinueRunningTimer(new TimeEntryViewModel(entry));
                    Device.StartTimer(new TimeSpan(0, 0, 0, 1), UpdateClock);
                    break;
                }

                //Find collection with same date as start date of entry
                TimeEntryListElementOverservableCollection correspondingCollection =
                    _vm.TimeEntries.FirstOrDefault(x => x.Date.Equals(entry.StartDateTime.Date));

                //if collection for date does not exist, create one
                if(correspondingCollection == null)
                {
                    correspondingCollection = new TimeEntryListElementOverservableCollection(entry.StartDateTime.Date);
                    _vm.TimeEntries.Add(correspondingCollection);
                }
            

                //Find corresponding parent entry 
                TimeEntryParent entryParent = null;
                foreach (var parent in correspondingCollection)
                {
                    if (parent.Ticket != null && ((parent as TimeEntryParent).BillCustomer == entry.BillCustomer)
                        && (parent.Ticket.ProjectURI.Equals(entry.TicketURI) 
                            || (parent.Ticket.uri != null &&  parent.Ticket.uri.Equals(entry.TicketURI) )))
                    {
                        entryParent = parent as TimeEntryParent;
                        break;
                    }
                }
                //parent entry not found, make a new one
                if (entryParent == null)
                {
                    entryParent = new TimeEntryParent();
                    entryParent.Date = entry.StartDateTime;
                    entryParent.RepliconTicketID = entry?.TicketURI;
                    //add parent to collection
                    correspondingCollection.Add(entryParent);
                }

                //create new entryVM
                TimeEntryViewModel entryVM = new TimeEntryViewModel(entry);

                //set parent on child entry
                entryVM.parent = entryParent;

                //add entry to parent
                entryParent.Entries.Add(entryVM);
              

            }
        }

        

        /// <summary>
        /// Handles when start/stop button is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerButton_OnClicked(object sender, EventArgs e)
        {
            //Start or stop timer
            _vm.TimerClicked();

            //reset selected ticket if stopping timer
            if (_vm.CurrentTimeEntry.IsRunning)
            {
                //update the UI every second to update the clock
                Device.StartTimer(new TimeSpan(0, 0, 0, 1), UpdateClock);
            }
        }

        /// <summary>
        /// Updates the timer label 
        /// </summary>
        /// <returns></returns>
        private bool UpdateClock()
        {
            bool isRunning = _vm.CurrentTimeEntry.IsRunning;
            if (isRunning)
            {
                var elapsedTime = DateTime.Now - _vm.CurrentTimeEntry.StartTime;

                if (!TimerLabel.IsFocused)
                {
                    TimerLabel.Text = $"{elapsedTime.Hours}:{elapsedTime.Minutes.NormalizeIntForTime()}:{elapsedTime.Seconds.NormalizeIntForTime()}";
                }
            }
            else
            {
                TimerLabel.Text = "0:00:00";
            }

            //will continue so long as timer is running
            return isRunning;
        }

        private void OnContinueEntry(ITimeEntryListElement TimeEntryVM)
        {
            _vm.ContinueTimerClicked(TimeEntryVM);

            //reset selected ticket if stopping timer
            if (!_vm.CurrentTimeEntry.IsRunning)
            {
                // AutoCompleteList.ClearSelectedValue();
            }
            else
            {
                //update the UI every second to update the clock
                Device.StartTimer(new TimeSpan(0, 0, 0, 1), UpdateClock);
            }
        }


        private  void LoadTickets_OnClicked(object sender, EventArgs e)
        {

            // The Progress<T> constructor captures our UI context,
            //  so the lambda will be run on the UI thread.
            var progress = new Progress<double>(percent =>
            {
                _vm.SyncProgress = percent;
            });

            bool success = false;
            // DoProcessing is run on the thread pool.
             Task.Run( async () =>
             {
                 success = await _vm.LoadTickets(progress);
                 if (!success)
                 {
                     _ui.Post(async (state) =>
                     {
                         await DisplayAlert("Attention", "Unable to sync tickets\nPlease check your connection and try again", "Ok");

                     }, null);
                 }
             });

           


        }

       


        private void TimerLabel_OnUnfocused(object sender, FocusEventArgs e)
        {
            if (!string.IsNullOrEmpty(TimerLabel.Text))
            {
                var updatedTime = TimerLabel.Text.ParseToDateTime();
                if (updatedTime != TimeSpan.MinValue)
                {
                    _vm.CurrentTimeEntry.StartTime = DateTime.Now - TimerLabel.Text.ParseToDateTime();

                }
            }
            
            //update label
            TimerLabel.Text = _vm.CurrentTimeEntry.RunTimeText;
        }


        private void TimerLabel_OnFocused(object sender, FocusEventArgs e)
        {
            TimerLabel.Text = "";
        }
    }
}
