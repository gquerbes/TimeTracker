using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace TimeTracker.Services
{
    public class RepliConnect
    {

        public static object GetServerData(string requestMethod ="GET" )
        {
            //ignore http errors
#warning This is due to self signed cert, should fix
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            WebRequest req = WebRequest.Create($"{Credentials.RepliConnectURL}/api/values");
            req.Method = requestMethod;
            WebResponse response = null;
            StreamReader reader = null;
            try
            {
                response = req.GetResponse();

                Stream dataStream = response.GetResponseStream();

                reader = new StreamReader(dataStream);

                var responseFromServer = reader.ReadToEnd();
                return responseFromServer;
            }
            catch (Exception e)
            {
                return e.Message + e.Data;
            }
            //var response = 
        }


        public static object GetValues()
        {
            return GetServerData("GET");
        }

       
    }
}
