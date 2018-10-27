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
        }

        private async void LoadData()
        {
            var entries = await App.Database.GetItemsAsync<TimeEntry>();
            foreach (var entry in entries)
            {
                //Find collection with same date as start date of entry
                TimeEntryObservableCollection correspondingCollection =
                    _vm.TimeEntries.FirstOrDefault(x => x.Date.Equals(entry.StartDate));

                //if collection does not exist, create one
                if(correspondingCollection == null)
                {
                    correspondingCollection = new TimeEntryObservableCollection(entry.StartDate);
                    _vm.TimeEntries.Add(correspondingCollection);
                }
                correspondingCollection.Add(new TimeEntryViewModel(entry));
            }
        }

        private void TimerButton_OnClicked(object sender, EventArgs e)
        {
            _vm.TimerClicked();
            Device.StartTimer(new TimeSpan(0, 0, 0, 1), UpdateClock);
        }

        private bool UpdateClock()
        {
            var elapsedTime = _vm.CurrentTimeEntry.Stopwatch.Elapsed;
            TimerLabel.Text = $"{elapsedTime.Hours}:{elapsedTime.Minutes}:{elapsedTime.Seconds}";

            return _vm.CurrentTimeEntry.Stopwatch.IsRunning;
        }


     
    }
}
