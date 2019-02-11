using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TimeTracker.Models;
using TimeTracker.Views;
using Xamarin.Forms;

namespace TimeTracker
{
    public partial class MainPage : ContentPage
    {
        private MainPageViewModel _vm;
        public MainPage()
        {
            _vm = new MainPageViewModel();
            this.BindingContext = _vm;

            InitializeComponent();
            LoadData();
            EntryListView.OnContinueEntry += OnContinueEntry;
        }

       

        /// <summary>
        /// Load data from the DB and add to table
        /// </summary>
        private void LoadData()
        {
            //Query entries
            var entries =  App.Database.GetItems<TimeEntry>();
            foreach (var entry in entries)
            {
                //Find collection with same date as start date of entry
                TimeEntryObservableCollection correspondingCollection =
                    _vm.TimeEntries.FirstOrDefault(x => x.Date.Equals(entry.StartDateTime.Date));

                //if collection does not exist, create one
                if(correspondingCollection == null)
                {
                    correspondingCollection = new TimeEntryObservableCollection(entry.StartDateTime.Date);
                    _vm.TimeEntries.Add(correspondingCollection);
                }
                correspondingCollection.Add(new TimeEntryViewModel(entry));
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

        /// <summary>
        /// Updates the timer label 
        /// </summary>
        /// <returns></returns>
        private bool UpdateClock()
        {
            var elapsedTime =  DateTime.Now - _vm.CurrentTimeEntry.StartTime;
            TimerLabel.Text = $"{elapsedTime.Hours}:{elapsedTime.Minutes}:{elapsedTime.Seconds}";

            //will continue so long as timer is running
            return _vm.CurrentTimeEntry.IsRunning;
        }

        private void OnContinueEntry(TimeEntryViewModel TimeEntryVM)
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


        private async void MenuItem_OnClicked(object sender, EventArgs e)
        {
            var page = new TestPage();
            await this.Navigation.PushAsync(page);
        }
    }
}
