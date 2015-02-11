using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using RestSharp;
using WPMGMT.BESScraper.Model;

namespace WPMGMT.BESScraper
{
    class BesApi
    {
        // Fields
        private Uri baseURL;
        private HttpBasicAuthenticator authenticator;

        // Properties
        public Uri BaseURL
        {
            get { return this.baseURL; }
            private set { this.baseURL = value; }
        }

        public HttpBasicAuthenticator Authenticator
        {
            get { return this.authenticator; }
            private set { this.authenticator = value; }
        }

        // Constructors
        public BesApi(Uri aBaseURL, string aUsername, string aPassword)
        {
            // Use to ignore SSL errors if specified in App.config
            if (AppSettings.Get<bool>("IgnoreSSL"))
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            }

            this.BaseURL = aBaseURL;
            this.authenticator = new HttpBasicAuthenticator(aUsername, aPassword);
        }

        public Actions GetActions()
        {
            RestClient client = new RestClient(this.BaseURL);
            client.Authenticator = this.Authenticator;

            RestRequest request = new RestRequest("actions", Method.GET);

            // Execute the request
            IRestResponse<List<WPMGMT.BESScraper.Model.Action>> response = client.Execute<List<WPMGMT.BESScraper.Model.Action>>(request);

            return Execute<Actions>(request);
        }

        public ActionDetail GetActionDetail(int id)
        {
            RestClient client = new RestClient(this.BaseURL);
            client.Authenticator = this.Authenticator;

            RestRequest request = new RestRequest("action/{id}/status", Method.GET);
            request.AddUrlSegment("id", id.ToString());

            // Execute the request
            IRestResponse<ActionDetail> response = client.Execute<ActionDetail>(request);

            try
            {
                // If the response contains an Exception
                if (response.ErrorException != null)
                {
                    // Throw it back up
                    throw response.ErrorException;
                }
                else
                {
                    return response.Data;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception encountered: {0}", ex.Message);
                return null;
            }
        }

        public T Execute<T>(RestRequest request) where T : new()
        {
            RestClient client = new RestClient();
            client.BaseUrl = this.BaseURL;
            client.Authenticator = this.Authenticator;

            var response = client.Execute<T>(request);

            try
            {
                if (response.ErrorException != null)
                {
                    throw new Exception(response.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error encountered: {0}", ex.Message);
            }

            return response.Data;
        }
    }
}
