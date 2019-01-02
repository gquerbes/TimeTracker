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
	        TimeEntryViewModel selectedTimeEntry = (sender as Button)?.BindingContext as TimeEntryViewModel;
	    }

	    private void DeleteContextAction_Clicked(object sender, EventArgs e)
	    {
            //get entry that was selected to remove
	        TimeEntryViewModel selectedTimeEntry = (sender as MenuItem)?.BindingContext as TimeEntryViewModel;

            //find corresponding collection
            var correspondingCollection = (this.BindingContext as MainPageViewModel)?.TimeEntries.Where((x => x.Date.Equals(selectedTimeEntry?.StartTime))).FirstOrDefault();

            //remove item from list
            correspondingCollection?.Remove(selectedTimeEntry);
            //delete item from DB
            selectedTimeEntry?.Delete();
           
        }
    }
}