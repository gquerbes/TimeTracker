using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.OidcClient;

namespace TimeTracker.Services.Authentication
{
   public  interface IAuthenticationService
   {
       Task<LoginResult> Authenticate();
   }
}
