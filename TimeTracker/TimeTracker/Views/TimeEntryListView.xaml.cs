using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracker.ViewModels;
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
	        //TimeEntryParent selectedTimeEntry = (sender as MenuItem)?.BindingContext as TimeEntryParent;

         //   //find corresponding collection
         //   var correspondingCollection = (this.BindingContext as MainPageViewModel)?.TimeEntriesParentsParents.Where((x => x.Date.Equals(selectedTimeEntry?.))).FirstOrDefault();

         //   //remove item from list
         //   correspondingCollection?.Remove(selectedTimeEntry);


         //   //delete item from DB
         //   selectedTimeEntry?.Delete();
           
        }

	    private void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
	    {
	        this.Navigation.PushAsync(new TimeEntryDetailPage(e.Item as TimeEntryViewModel));
	    }
	}
}