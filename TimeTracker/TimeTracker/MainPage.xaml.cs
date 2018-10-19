using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TimeTracker
{
    public partial class MainPage : ContentPage
    {
        private MainPageViewModel _vm;
        public MainPage()
        {
            _vm = new MainPageViewModel();
            this.BindingContext = _vm;

            InitializeComponent();
        }
    }
}
