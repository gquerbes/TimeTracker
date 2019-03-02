using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracker.Interfaces;
using TimeTracker.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeTracker.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TimeEntryListView : ContentView
    {
        public delegate void ContinueEntry(ITimeEntryListElement TimeEntryVM);

        public ContinueEntry OnContinueEntry;

        public delegate void ExpandCollapseParent(TimeEntryParent TimeEntryParent);

        public ExpandCollapseParent OnExpandCollapseParent;


        public TimeEntryListView()
        {
            InitializeComponent();
        }


        private void ContinueJobTimer_OnClicked(object sender, EventArgs e)
        {
            ITimeEntryListElement selectedTimeEntry = (sender as Button)?.BindingContext as ITimeEntryListElement;
            OnContinueEntry?.Invoke(selectedTimeEntry);
        }

        private async void DeleteParentContextAction_Clicked(object sender, EventArgs e)
        {
            //get parent entry that was selected to be removed
            TimeEntryParent selectedParent = (sender as MenuItem)?.BindingContext as TimeEntryParent;

            if (selectedParent.Entries.Count > 1)
            {
                var response = await Application.Current.MainPage.DisplayAlert("Attention",
                     $"Are you sure you want to delete all {selectedParent.Entries.Count()} entries?", "yes", "cancel");

                //user did not approve, return
                if (!response)
                {
                   return;
                }
            }

            //find corresponding collection
            var correspondingCollection =
                (this.BindingContext as MainPageViewModel)?.TimeEntries.FirstOrDefault(x =>
                    x.Date.Date.Equals(selectedParent?.Date.Date));

            //for each child, remove it from the list and delete from database
            foreach (var entry in selectedParent.Entries)
            {
                entry.Delete();

                if (correspondingCollection.Contains(entry))
                {
                    correspondingCollection.Remove(entry);
                }
            }
            //remove parent from list
            correspondingCollection.Remove(selectedParent);
        }

        private void DeleteChildContextAction_Clicked(object sender, EventArgs e)
        {
            //get entry that was selected to remove
            TimeEntryViewModel selectedTimeEntry = (sender as MenuItem)?.BindingContext as TimeEntryViewModel;

            //find corresponding collection
            var correspondingCollection =
                (this.BindingContext as MainPageViewModel)?.TimeEntries.FirstOrDefault(x =>
                    x.Date.Equals(selectedTimeEntry?.StartTime.Date));

            //remove item from list
            correspondingCollection?.Remove(selectedTimeEntry);

            //remove from parent
            selectedTimeEntry.parent.Entries.Remove(selectedTimeEntry);

            //delete item from DB
            selectedTimeEntry?.Delete();

            //remove timeEntryParent from list if no children remain.
            if (!selectedTimeEntry.parent.Entries.Any())
            {
                correspondingCollection?.Remove(selectedTimeEntry.parent);
            }

            //update view cell if still visible
            else
            {
                selectedTimeEntry.parent.OnPropertyChanged(null);
            }

        }

        private void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is TimeEntryParent)
            {
                TimeEntryParent parent = e.Item as TimeEntryParent;

                if (parent.Entries.Count > 1)
                {
                    OnExpandCollapseParent?.Invoke(parent);
                }

                else
                {
                    Navigation.PushAsync(new TimeEntryDetailPage(parent.Entries.FirstOrDefault()));
                }
            }
            else
            {
                this.Navigation.PushAsync(new TimeEntryDetailPage(e.Item as TimeEntryViewModel));
            }

        }
    }
    public class EntryListViewDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ParentTemplate { get; set; }
        public DataTemplate ChildTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return ((ITimeEntryListElement)item) is TimeEntryParent ? ParentTemplate : ChildTemplate;
        }
    }
}