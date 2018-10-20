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
using Xamarin.Forms;

namespace TimeTracker
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private Entry _currentEntry;
        private Entry CurrentEntry
        {
            get
            {
                if (_currentEntry == null)
                {
                    _currentEntry = new Entry();
                }

                return _currentEntry;
            }
        }
        public string CurrentComments
        {
            get => CurrentEntry.Comments;
            set => CurrentEntry.Comments = value;
        }

        public string CurrentTimer
        {
            get
            {
                var elapsedTime = Stopwatch.Elapsed;
                return $"{elapsedTime.Hours}:{elapsedTime.Minutes}:{elapsedTime.Seconds}";
            } 

        }

        public Stopwatch Stopwatch = new Stopwatch();

        public ObservableCollection<ObservableCollection<Entry>> _entries;

        public ObservableCollection<ObservableCollection<Entry>> Entries
        {
            get
            {
                if (_entries == null)
                {
                    _entries = new ObservableCollection<ObservableCollection<Entry>>{new ObservableCollection<Entry>()};
                }

                return _entries;
            }
        }



        public void TimerClicked()
        {
            if (!Stopwatch.IsRunning)
            {
                Stopwatch.Start();
            }
            else
            {
                Stopwatch.Stop();
                CurrentEntry.RunTime = Stopwatch.Elapsed;
                Stopwatch.Reset();
                Entries.First().Add(CurrentEntry);
               _currentEntry = null;
                OnPropertyChanged();
            }

        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


}
