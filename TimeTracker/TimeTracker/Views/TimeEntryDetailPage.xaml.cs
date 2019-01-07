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
	public partial class TimeEntryDetailPage : ContentPage
	{
	    private TimeEntryViewModel vm;
		public TimeEntryDetailPage (TimeEntryViewModel vm)
		{
		    this.vm = vm;
		    this.BindingContext = vm;
			InitializeComponent ();

		}
	}
}