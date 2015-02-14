using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Xml.Linq;
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


        // Methods
        public WPMGMT.BESScraper.Model.Action GetAction(int id)
        {
            return GetActions().SingleOrDefault(x => x.ID == id);
        }

        public List<WPMGMT.BESScraper.Model.Action> GetActions()
        {
            RestClient client = new RestClient(this.BaseURL);
            client.Authenticator = this.Authenticator;

            RestRequest request = new RestRequest("actions", Method.GET);

            return Execute<List<WPMGMT.BESScraper.Model.Action>>(request);
        }

        public ActionDetail GetActionDetail(int id)
        {
            RestClient client = new RestClient(this.BaseURL);
            client.Authenticator = this.Authenticator;

            RestRequest request = new RestRequest("action/{id}/status", Method.GET);
            request.AddUrlSegment("id", id.ToString());

            // Execute the request
            XDocument response = Execute(request);

            ActionDetail result = new ActionDetail(
                            Int32.Parse(response.Element("BESAPI").Element("ActionResults").Element("ActionID").Value.ToString()),
                            response.Element("BESAPI").Element("ActionResults").Element("Status").Value.ToString(),
                            response.Element("BESAPI").Element("ActionResults").Element("DateIssued").Value.ToString());

            return result;
        }

        public List<ActionResult> GetActionResults(int id)
        {
            List<ActionResult> results = new List<ActionResult>();
            RestClient client = new RestClient(this.BaseURL);
            client.Authenticator = this.Authenticator;

            RestRequest request = new RestRequest("action/{id}/status", Method.GET);
            request.AddUrlSegment("id", id.ToString());

            // Execute the request
            XDocument response = Execute(request);

            foreach (XElement computerElement in response.Element("BESAPI").Element("ActionResults").Elements("Computer"))
            {
                results.Add(new ActionResult(
                                    id,                                                                         // Action ID
                                    Int32.Parse(computerElement.Attribute("ID").Value.ToString()),              // Computer ID
                                    computerElement.Element("Status").Value.ToString(),                         // Status
                                    Int32.Parse(computerElement.Element("ApplyCount").Value.ToString()),        // Times applied
                                    Int32.Parse(computerElement.Element("RetryCount").Value.ToString()),        // Times retried
                                    Int32.Parse(computerElement.Element("LineNumber").Value.ToString()),        // Which script line is being executed
                                    Convert.ToDateTime(computerElement.Element("StartTime").Value.ToString()),  // Time execution started
                                    Convert.ToDateTime(computerElement.Element("EndTime").Value.ToString())     // Time execution ended
                    ));
            }

            return results;
        }

        public List<Computer> GetComputers()
        {
            RestClient client = new RestClient(this.BaseURL);
            client.Authenticator = this.Authenticator;

            RestRequest request = new RestRequest("computers", Method.GET);

            return Execute<List<Computer>>(request);
        }

        public List<ComputerGroup> GetComputerGroups()
        {
            List<ComputerGroup> groups = new List<ComputerGroup>();

            foreach (Site site in GetSites())
            {
                groups.AddRange(GetComputerGroups(site.Name));
            }

            return groups;
        }

        public List<ComputerGroup> GetComputerGroups(string site)
        {
            List<ComputerGroup> groups = new List<ComputerGroup>();

            RestClient client = new RestClient(this.BaseURL);
            client.Authenticator = this.Authenticator;

            RestRequest request = new RestRequest("computergroups/{site}", Method.GET);
            request.AddUrlSegment("site", site);

            XDocument response = Execute(request);

            foreach(XElement groupElement in response.Element("BESAPI").Elements("ComputerGroup"))
            {
                groups.Add(GetComputerGroup(site, Int32.Parse(groupElement.Element("ID").Value)));
            }

            return groups;
        }

        public ComputerGroup GetComputerGroup(string site, int id)
        {
            RestClient client = new RestClient(this.BaseURL);
            client.Authenticator = this.Authenticator;

            RestRequest request = new RestRequest("computergroups/{site}/{id}", Method.GET);
            request.AddUrlSegment("site", site);
            request.AddUrlSegment("id", id.ToString());

            ComputerGroup group = Execute<ComputerGroup>(request);
            group.GroupID = id;

            return group;
        }

        public List<Site> GetSites()
        {
            List<Site> sites = new List<Site>();

            RestClient client = new RestClient(this.BaseURL);
            client.Authenticator = this.Authenticator;

            RestRequest request = new RestRequest("sites", Method.GET);

            // Execute the request
            XDocument response = Execute(request);

            foreach (XElement siteElement in response.Element("BESAPI").Elements())
            {
                sites.Add(new Site(siteElement.Element("Name").Value.ToString(), siteElement.Name.ToString()));
            }

            return sites;
        }

        public XDocument Execute(RestRequest request)
        {
            RestClient client = new RestClient();
            client.BaseUrl = this.BaseURL;
            client.Authenticator = this.Authenticator;

            IRestResponse response = client.Execute(request);

            try
            {
                if (response.ErrorException != null)
                {
                    throw new Exception(response.ErrorMessage);
                }

                // Return non-deserialized XML document
                return XDocument.Parse(response.Content, LoadOptions.None);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error encountered: {0}", ex.Message);
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
