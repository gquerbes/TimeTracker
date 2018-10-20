using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        }

        private void TimerButton_OnClicked(object sender, EventArgs e)
        {
            _vm.TimerClicked();
            Device.StartTimer(new TimeSpan(0, 0, 0, 1), UpdateClock);
        }

        private bool UpdateClock()
        {
            var elapsedTime = _vm.Stopwatch.Elapsed;
            TimerLabel.Text = $"{elapsedTime.Hours}:{elapsedTime.Minutes}:{elapsedTime.Seconds}";

            return _vm.Stopwatch.IsRunning;
        }
    }
}
