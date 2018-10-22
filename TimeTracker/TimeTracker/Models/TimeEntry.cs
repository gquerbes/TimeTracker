using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using SQLite;
using TimeTracker.Annotations;

namespace TimeTracker
{
    public class TimeEntry : INotifyPropertyChanged
    {
        #region Properties

        private TimeSpan _runTime;
        public TimeSpan RunTime
        {
            get
            {
                if (_runTime.Equals(TimeSpan.Zero))
                {
                    return Stopwatch.Elapsed;
                }

                return _runTime;
            }
            set => _runTime = value;
        }
       
        public string Comments { get; set; }

        public Stopwatch Stopwatch;

        public string Guid { get; set; }
        #endregion

        #region Constructor

        public TimeEntry()
        {
            Stopwatch = new Stopwatch();
        }
        

        #endregion

        #region BindedProperties
        [Ignore]
        public string RunTimeText
        {
            get { return $"{RunTime.Hours}:{RunTime.Minutes}:{RunTime.Seconds}"; }
        }
        #endregion

        #region Methods

        public void StartTimer()
        {
            if (!Stopwatch.IsRunning)
            {
                Stopwatch.Start();
            }
            else
            {
                throw new Exception("Stopwatch already running!");
            }
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


        private async void Save()
        {
            await App.Database.SaveItemAsync(this);
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
