using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
	            ((AutoCompleteList)bindable).SelectedItemLabel.Text = $"{((AutoCompleteList)bindable).SelectedTicket.name} : {((AutoCompleteList)bindable).SelectedTicket.description}";
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