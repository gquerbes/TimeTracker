using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using TimeTracker.Annotations;
using TimeTracker.Database;
using Xamarin.Forms;

namespace TimeTracker.ViewModels
{
    public class AutoCompleteListViewModel : INotifyPropertyChanged
    {
        private bool isListViewVisible;
        public bool IsListViewVisible
        {
            get
            {
                return isListViewVisible;
            }
            set
            {
                if (isListViewVisible != value)
                {
                    isListViewVisible = value;
                    OnPropertyChanged();
                }
            }
        }

        private List<Ticket> _tickets = new List<Ticket>();

        public List<Ticket> Tickets
        {
            get
            {
                if (!_tickets.Any())
                {
                    _tickets = App.Database.GetItems<Ticket>();
                }

                return _tickets;
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
