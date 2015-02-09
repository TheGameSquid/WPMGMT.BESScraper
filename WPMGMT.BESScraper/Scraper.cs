using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using RestSharp;
using WPMGMT.BESScraper.Model;

namespace WPMGMT.BESScraper
{
    class Scraper
    {
        // Fields
        private string baseURL;
        private HttpBasicAuthenticator authenticator;

        // Properties
        public string BaseURL
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
        public Scraper(string aBaseURL, string aUsername, string aPassword)
        {
            // Use to ignore SSL errors if specified in App.config
            if (AppSettings.Get<bool>("IgnoreSSL"))
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            }

            this.BaseURL = aBaseURL;
            this.authenticator = new HttpBasicAuthenticator(aUsername, aPassword);
        }

        public List<WPMGMT.BESScraper.Model.Action> GetActions()
        {
            RestClient client = new RestClient(this.BaseURL);
            client.Authenticator = this.Authenticator;

            RestRequest request = new RestRequest("actions", Method.GET);

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

        //public List<WPMGMT.BESScraper.Model.ActionDetail> GetActionDetails()
        //{
        //    RestClient client = new RestClient(this.BaseURL);
        //    client.Authenticator = this.Authenticator;

        //    List<WPMGMT.BESScraper.Model.Action> actions = this.GetActions();

        //    foreach (WPMGMT.BESScraper.Model.Action action in actions)
        //    {

        //    }

        //    RestRequest request = new RestRequest("actions", Method.GET);

        //    // Execute the request
        //    IRestResponse<List<WPMGMT.BESScraper.Model.Action>> response = client.Execute<List<WPMGMT.BESScraper.Model.Action>>(request);

        //    try
        //    {
        //        // If the response contains an Exception
        //        if (response.ErrorException != null)
        //        {
        //            // Throw it back up
        //            throw response.ErrorException;
        //        }
        //        else
        //        {
        //            return response.Data;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Exception encountered: {0}", ex.Message);
        //        return null;
        //    }
        //}
    }
}
