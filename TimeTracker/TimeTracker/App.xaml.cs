using System;
using System.IO;
using TimeTracker.Database;
using TimeTracker.Services.Authentication;
using TimeTracker.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace TimeTracker
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new TimeTracker.Views.Main.MainPage();
        }

        private static AppDatabase database;

        public static AppDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new AppDatabase(Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "app.db3"));
                }

                return database;
            }
        }

        public static async void Authenticate()
        {
            var authenticationService = DependencyService.Get<IAuthenticationService>();
            var loginResult = await authenticationService.Authenticate();

            //handle token stuff

            //notify the UI
        }
        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
