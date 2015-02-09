using System;
using System.Collections.Generic;
using System.Net;
using RestSharp;

using WPMGMT.BESScraper.Model;

namespace WPMGMT.BESScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            // Use to ignore SSL errors
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

            var client = new RestClient("https://pc120006933:52311/api/");
            client.Authenticator = new HttpBasicAuthenticator("iemadmin", "bigfix");

            RestRequest request = new RestRequest("actions", Method.GET);
            //request.AddParameter("actions", "");

            // Execute the request
            IRestResponse<List<WPMGMT.BESScraper.Model.Action>> response = client.Execute<List<WPMGMT.BESScraper.Model.Action>>(request);

            try
            {
                // If the response contains an Exception
                if (response.ErrorException != null)
                {
                    // Throw it back up
                    throw response.ErrorException;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception encountered: {0}", ex.Message);
            }

            Console.Read();
        }
    }
}
