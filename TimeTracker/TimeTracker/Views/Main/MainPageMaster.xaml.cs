using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeTracker.Views.Main
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPageMaster : ContentPage
    {

        public delegate void UpdateTicketsDelegate();

        public UpdateTicketsDelegate OnUpdateTickets;
        public MainPageMaster()
        {
            InitializeComponent();

            BindingContext = new MainPageMasterViewModel();

        }

        class MainPageMasterViewModel : INotifyPropertyChanged
        {
      
            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }

        private void LoginButton_OnClicked(object sender, EventArgs e)
        {
            try
            {
                App.Authenticate();
            }
            catch
            {
                //DisplayAlert()
            }
        }

        private void UpdateTicketsList_OnClicked(object sender, EventArgs e)
        {
            OnUpdateTickets?.Invoke();
        }
    }
}