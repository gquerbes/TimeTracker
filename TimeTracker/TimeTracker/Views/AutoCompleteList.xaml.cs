using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracker.Database;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeTracker.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AutoCompleteList : ContentView
	{

	    public delegate void TicketSelected(Ticket ticket);

	    public TicketSelected OnTicketSelected;

		public AutoCompleteList ()
		{
			InitializeComponent ();

		    TicketListView.ItemTapped += (sender, args) =>
		    {
		        Ticket ticket = args.Item as Ticket;

                OnTicketSelected?.Invoke(ticket);
		        SelectedTicket = ticket;
		        SearchBar.Text = "";
		    };
           SearchBar.TextChanged += (sender, args) => { SearchTextChanged(args); }; 
		}

	  


        #region Bindable Selected Ticket Property

	    public static readonly BindableProperty SelectedTicketProperty =
	        BindableProperty.Create("SelectedTicket", typeof(Ticket), typeof(AutoCompleteList), null, BindingMode.TwoWay, propertyChanged: OnSelectedTicketChanged);

	    static void OnSelectedTicketChanged(BindableObject bindable, object oldValue, object newValue)
	    {
	        if (newValue == null)
	        {
	            ((AutoCompleteList)bindable).SelectedItemStack.IsVisible = false;
	            ((AutoCompleteList)bindable).SearchBar.IsVisible = true;
            }
	        else
	        {
	            Ticket ticket = ((Ticket)newValue);
	            ((AutoCompleteList)bindable).SelectedItemStack.IsVisible = true;
	            ((AutoCompleteList)bindable).SelectedItemLabel.Text = $"{((AutoCompleteList)bindable).SelectedTicket.key} : {((AutoCompleteList)bindable).SelectedTicket.Summary}";
	            ((AutoCompleteList)bindable).SearchBar.IsVisible = false;
	        }
	    }
	    public Ticket SelectedTicket
	    {
	        get
	        {
	            return (Ticket)GetValue(SelectedTicketProperty);
	        }
	        set
	        {
	            SetValue(SelectedTicketProperty, value);
	        }
	    }

        #endregion

        #region Tickets

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

        #endregion

        #region Methods
	    public void SearchTextChanged(TextChangedEventArgs args)
	    {
	        if (string.IsNullOrEmpty(args.NewTextValue))
	        {
	            _filteredTickets = Tickets;
	            return;
	        }

	        FilteredTickets = Tickets.Where(x => !string.IsNullOrEmpty(x.key) && x.key.ToLower().Contains(args?.NewTextValue?.ToLower())).ToList();
	        //do this better instead of changing item source constantly on filter
	        TicketListView.ItemsSource = FilteredTickets;
	    }

	    public void ClearSelectedTicket_OnClicked(object sender, EventArgs e)
	    {
	        SelectedTicket = null;
	    }

	    private void SearchBar_OnFocused(object sender, FocusEventArgs e)
	    {
	        TicketListView.IsVisible = true;
	    }

	    private void SearchBar_OnUnfocused(object sender, FocusEventArgs e)
	    {
	        TicketListView.IsVisible = false;

	    }


        #endregion

    }
}