using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
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

            foreach (var x in App.Current.Properties)
            {
                var obj = JsonConvert.DeserializeObject<TimeEntry>(x.Value as string);

                _vm.TimeEntries.FirstOrDefault()?.Add(obj);
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
