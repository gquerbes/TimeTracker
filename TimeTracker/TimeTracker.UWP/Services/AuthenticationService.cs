using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auth0.OidcClient;
using IdentityModel.OidcClient;
using TimeTracker.Services.Authentication;
using TimeTracker.UWP.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(AuthenticationService))]

namespace TimeTracker.UWP.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private Auth0Client _auth0Client;

        public AuthenticationService()
        {
            _auth0Client = new Auth0Client(new Auth0ClientOptions
            {
                Domain = AuthenticationConfig.Domain,
                ClientId = AuthenticationConfig.ClientId,
                
            });
           
        }

        public async Task<LoginResult> Authenticate()
        {
            var loginResult = await _auth0Client.LoginAsync(new { audience = "https://ausa.abas.cloud/", connection = "ausa" });
            return loginResult;
        }
    }
}
