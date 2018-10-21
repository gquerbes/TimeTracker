using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeTracker.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TimeEntryListView : ContentView
	{
		public TimeEntryListView ()
		{
			InitializeComponent ();
		}

	    private void PreviousJobTimer_OnClicked(object sender, EventArgs e)
	    {
	        TimeEntry selectedTimeEntry = (sender as Button)?.BindingContext as TimeEntry;
	    }
    }
}