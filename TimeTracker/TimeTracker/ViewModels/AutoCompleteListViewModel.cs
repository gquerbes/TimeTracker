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

        private List<Ticket> _filteredTickets;
        public List<Ticket> FilteredTickets
        {
            get
            {
                if (_filteredTickets == null)
                {
                    _filteredTickets = Tickets;
                }

                return _filteredTickets;
            }
            protected set { _filteredTickets = value; }
        }

        #region ListViewSearching

        public void SearchTextChanged(TextChangedEventArgs args)
        {
            if (string.IsNullOrEmpty(args.NewTextValue))
            {
                _filteredTickets = Tickets;
                return;
            }

            FilteredTickets = Tickets.Where(x => x.key.ToLower().Contains(args.NewTextValue.ToLower())).ToList();
            OnPropertyChanged(nameof(FilteredTickets));
        }


        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
