﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracker.Annotations;
using TimeTracker.Database;
using TimeTracker.Models.Replicon.RepliconReply;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeTracker.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AutoCompleteList : ContentView
	{

	    public delegate void TicketSelected(RepliconTask ticket);

	    public TicketSelected OnTicketSelected;

		public AutoCompleteList ()
		{
			InitializeComponent ();

		    TicketListView.ItemTapped += (sender, args) =>
		    {
		        RepliconTask ticket = args.Item as RepliconTask;

                OnTicketSelected?.Invoke(ticket);
		        SelectedTicket = ticket;
		        SearchBar.Text = "";
		    };
           SearchBar.TextChanged += (sender, args) => { SearchTextChanged(args); };

        
		}

	  


        #region Bindable Selected Ticket Property

	    public static readonly BindableProperty SelectedTicketProperty =
	        BindableProperty.Create("SelectedTicket", typeof(RepliconTask), typeof(AutoCompleteList), null, BindingMode.TwoWay, propertyChanged: OnSelectedTicketChanged);

	    static void OnSelectedTicketChanged(BindableObject bindable, object oldValue, object newValue)
	    {
	        if (newValue == null)
	        {
	            ((AutoCompleteList)bindable).SelectedItemStack.IsVisible = false;
	            ((AutoCompleteList)bindable).SearchBar.IsVisible = true;
            }
	        else
	        {
                RepliconTask ticket = ((RepliconTask)newValue);
	            ((AutoCompleteList)bindable).SelectedItemStack.IsVisible = true;
                var descriptionText = ((AutoCompleteList) bindable).SelectedTicket.description;
                if (!string.IsNullOrEmpty(descriptionText))
                {
                    descriptionText = $": {descriptionText}";
                }
	            ((AutoCompleteList)bindable).SelectedItemLabel.Text = $"{((AutoCompleteList)bindable).SelectedTicket.name}{descriptionText}";
	            ((AutoCompleteList)bindable).SearchBar.IsVisible = false;
	        }
	    }
	    public RepliconTask SelectedTicket
	    {
	        get
	        {
	            return (RepliconTask)GetValue(SelectedTicketProperty);
	        }
	        set
	        {
	            SetValue(SelectedTicketProperty, value);
	        }
	    }

        #endregion

        #region Tickets

	    private List<RepliconTask> _tickets = new List<RepliconTask>();

	    public List<RepliconTask> Tickets
	    {
	        get
	        {
	            if (!_tickets.Any())
	            {
	                _tickets = App.Database.GetItems<RepliconTask>();
	            }

	            return _tickets;
	        }
	    }

	    private List<RepliconTask> _filteredTickets;
	    public List<RepliconTask> FilteredTickets
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

        #endregion

        #region Methods
	    public void SearchTextChanged(TextChangedEventArgs args)
	    {
	        if (string.IsNullOrEmpty(args.NewTextValue))
	        {
	            _filteredTickets = Tickets;
	            return;
	        }

	        FilteredTickets = Tickets.Where(x => !string.IsNullOrEmpty(x.name) && x.name.ToLower().Contains(args?.NewTextValue?.ToLower())).ToList();

            if (!FilteredTickets.Any())
            {
                FilteredTickets = Tickets.Where(x => !string.IsNullOrEmpty(x.description) && x.description.ToLower().Contains(args?.NewTextValue.ToLower())).ToList();
            }

            //nothing found? check the tickets by description
//TODO: Add Description filtering for overhead / training tickets

            //do this better instead of changing item source constantly on filter
            TicketListView.ItemsSource = FilteredTickets;
	    }

	    public void ClearSelectedTicket_OnClicked(object sender, EventArgs e)
	    {
	        SelectedTicket = null;
            SearchBar.Focus();
        }

        /// <summary>
        /// Used to trigger show /hide logic for comments text entry
        /// </summary>
        public bool TicketListIsVisible
        {
            get => TicketListView.IsVisible;
        }

	    private void SearchBar_OnTextChanged(object sender, TextChangedEventArgs e)
	    {
            //if new string is empty, hide list
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                TicketListView.IsVisible = false;
                OnPropertyChanged(nameof(TicketListIsVisible));
                return;
            }

            //else string is not empty and list has not item source, se the source
            if (TicketListView.ItemsSource == null)
            {
                TicketListView.ItemsSource = FilteredTickets;
            }

            if (!TicketListView.IsVisible)
            {
                TicketListView.IsVisible = true;
                OnPropertyChanged(nameof(TicketListIsVisible));
            }
        }

	    private void SearchBar_OnUnfocused(object sender, FocusEventArgs e)
	    {

	     //   TicketListView.IsVisible = false;
            OnPropertyChanged(nameof(TicketListIsVisible));
        }


        #endregion

    }
}