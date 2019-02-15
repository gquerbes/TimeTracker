using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using TimeTracker.Annotations;

namespace TimeTracker.ViewModels
{
    public class TimeEntryParent : INotifyPropertyChanged
    {
        /// <summary>
        /// Holds list of entries for a specific ordertime
        /// </summary>
        public List<TimeEntryViewModel> Entries { get; set; } = new List<TimeEntryViewModel>();


        public string Comments
        {
            get
            {
                if (Entries.Count > 1)
                {
                    return "**Multiple Entries***";
                }
                else
                {
                    return Entries.FirstOrDefault()?.Comments;
                }
            }
        }

        public string SelectedTicketLabel => Entries.FirstOrDefault()?.SelectedTicketLabel;

        public string RunTimeText
        {
            get
            {
                TimeSpan totalTime = new TimeSpan();
                foreach (var timeEntryViewModel in Entries)
                {
                    totalTime += timeEntryViewModel.RunTime;
                }

                return $"{totalTime.Hours}:{totalTime.Minutes}:{totalTime.Seconds}";
            }

        }




        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
