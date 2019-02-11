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
        public delegate void ContinueEntry(TimeEntryViewModel TimeEntryVM);

        public ContinueEntry OnContinueEntry;

		public TimeEntryListView()
		{
			InitializeComponent();
		}


	    private void ContinueJobTimer_OnClicked(object sender, EventArgs e)
	    {
	        TimeEntryViewModel selectedTimeEntry = (sender as Button)?.BindingContext as TimeEntryViewModel;
            OnContinueEntry?.Invoke(selectedTimeEntry);
	    }

	    private void DeleteContextAction_Clicked(object sender, EventArgs e)
	    {
            //get entry that was selected to remove
	        TimeEntryViewModel selectedTimeEntry = (sender as MenuItem)?.BindingContext as TimeEntryViewModel;

            //find corresponding collection
            var correspondingCollection = (this.BindingContext as MainPageViewModel)?.TimeEntries.Where((x => x.Date.Equals(selectedTimeEntry?.StartTime.Date))).FirstOrDefault();

            //remove item from list
            correspondingCollection?.Remove(selectedTimeEntry);
            //delete item from DB
            selectedTimeEntry?.Delete();
           
        }

	    private void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
	    {
	        this.Navigation.PushAsync(new TimeEntryDetailPage(e.Item as TimeEntryViewModel));
	    }
	}
}