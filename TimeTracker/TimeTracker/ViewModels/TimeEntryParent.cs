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

        public string RunTimeText => Entries.FirstOrDefault()?.RunTimeText;


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
