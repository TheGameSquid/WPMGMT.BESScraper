using System;
using System.Net;
using RestSharp;

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

            RestRequest request = new RestRequest("query", Method.GET);
            request.AddParameter("relevance", "(name of it, id of it, last report time of it) of bes computers whose (name of it = \"PO130021498\")");

            // execute the request
            IRestResponse response = client.Execute(request);
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

            var content = response.Content; // raw content as string

            Console.WriteLine(content);
            Console.Read();
        }
    }
}
