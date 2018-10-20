using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using TimeTracker.Annotations;

namespace TimeTracker
{
    public class TimeEntry : INotifyPropertyChanged
    {
        #region Properties

        public TimeSpan RunTime => Stopwatch.Elapsed;
       
        public string Comments { get; set; }

        public  Stopwatch Stopwatch;
        #endregion

        #region Constructor

        public TimeEntry()
        {
            Stopwatch = new Stopwatch();
        }
        

        #endregion

        #region BindedProperties

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
            }
            else
            {
                throw new Exception("Stopwatch was not running!");
            }
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
