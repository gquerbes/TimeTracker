using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracker.Database;
using TimeTracker.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeTracker.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AutoCompleteList : ContentView
	{
	    private AutoCompleteListViewModel vm;

	    public delegate void TicketSelected(Ticket ticket);

	    public TicketSelected OnTicketSelected;

		public AutoCompleteList ()
		{
			InitializeComponent ();
            vm= new AutoCompleteListViewModel();
		    this.BindingContext = vm;

		    TicketListView.ItemTapped += (sender, args) =>
		    {
		        Ticket ticket = args.Item as Ticket;

                OnTicketSelected?.Invoke(ticket);
		        vm.SelectedTicket = ticket;
		        SearchBar.Text = "";
		    };
            SearchBar.TextChanged += (sender, args) => { vm.SearchTextChanged(args); }; 
		}



	    public void SearchBarFocused(object x, EventArgs y)
	    {
	        vm.IsListViewVisible = true;
	    }

	    public void SearchBarUnfocused(object x, EventArgs y)
	    {
	        vm.IsListViewVisible = false;
	    }

	    private void ClearSelectedTicket_OnClicked(object sender, EventArgs e)
	    {
	        vm.SelectedTicket = null;
	    }
	}
}