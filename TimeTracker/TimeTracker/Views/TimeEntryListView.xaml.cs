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
		public TimeEntryListView()
		{
			InitializeComponent();
		}

	    private void PreviousJobTimer_OnClicked(object sender, EventArgs e)
	    {
	        TimeEntry selectedTimeEntry = (sender as Button)?.BindingContext as TimeEntry;
	    }

	    private void DeleteContextAction_Clicked(object sender, EventArgs e)
	    {
            //get entry that was selected to remove
	        TimeEntry selectedTimeEntry = (sender as MenuItem)?.BindingContext as TimeEntry;

            //find corresponding collection
            var correspondingCollection = (this.BindingContext as MainPageViewModel)?.TimeEntries.Where((x => x.Date.Equals(selectedTimeEntry?.StartDate))).FirstOrDefault();

            //remove item from list
            correspondingCollection?.Remove(selectedTimeEntry);
            //delete item from DB
            selectedTimeEntry?.Delete();
           
        }
    }
}