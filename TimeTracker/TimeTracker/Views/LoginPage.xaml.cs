using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auth0.OidcClient;
using TimeTracker.Services;
using TimeTracker.Services.Authentication;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeTracker.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private readonly IAuth0Client _auth0Client;
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            var authenticationService = DependencyService.Get<IAuthenticationService>();
            var loginResult = await authenticationService.Authenticate();


            var sb = new StringBuilder();

            if (loginResult.IsError)
            {
               // ResultLabel.Text = "An error occurred during login...";

                sb.AppendLine("An error occurred during login:");
                sb.AppendLine(loginResult.Error);
            }
            else
            {
             //   ResultLabel.Text = $"Welcome {loginResult.User.Identity.Name}";

                sb.AppendLine($"ID Token: {loginResult.IdentityToken}");
                sb.AppendLine($"Access Token: {loginResult.AccessToken}");
                sb.AppendLine($"Refresh Token: {loginResult.RefreshToken}");
                sb.AppendLine();
                sb.AppendLine("-- Claims --");
                foreach (var claim in loginResult.User.Claims)
                {
                    sb.AppendLine($"{claim.Type} = {claim.Value}");
                }
            }

            System.Diagnostics.Debug.WriteLine(sb.ToString());

        }
    }
}
