using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using TimeTracker.Annotations;
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

        public TimeEntry TimeEntry{get; set; }

      

        public TimeSpan RunTime
        {
            get
            {
                if (TimeEntry.RunTime.Equals(TimeSpan.Zero))
                {
                    return Stopwatch.Elapsed;
                }

                return TimeEntry.RunTime;
            }
            set => TimeEntry.RunTime = value;
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
                    return Color.Red;
                }

                return Color.Green;
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
                TimeEntry.StartDate = DateTime.Today;
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


        public async void Save()
        {
            await App.Database.SaveItemAsync(TimeEntry);
        }

        public async void Delete()
        {
            await App.Database.DeleteItemAsync(TimeEntry);
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
