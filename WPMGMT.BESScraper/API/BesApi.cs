using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using RestSharp;
using WPMGMT.BESScraper.Model;

namespace WPMGMT.BESScraper.API
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

        public ActionDetail GetActionDetail(WPMGMT.BESScraper.Model.Action action)
        {
            RestClient client = new RestClient(this.BaseURL);
            client.Authenticator = this.Authenticator;

            RestRequest request = new RestRequest("action/{id}/status", Method.GET);
            request.AddUrlSegment("id", action.ActionID.ToString());

            // Execute the request
            XDocument response = Execute(request);

            ActionDetail result = new ActionDetail(
                            Int32.Parse(response.Element("BESAPI").Element("ActionResults").Element("ActionID").Value.ToString()),
                            response.Element("BESAPI").Element("ActionResults").Element("Status").Value.ToString(),
                            response.Element("BESAPI").Element("ActionResults").Element("DateIssued").Value.ToString());

            return result;
        }

        public List<ActionDetail> GetActionDetails(List<WPMGMT.BESScraper.Model.Action> actions)
        {
            List<ActionDetail> details = new List<ActionDetail>();

            foreach (WPMGMT.BESScraper.Model.Action action in actions)
            {
                details.Add(GetActionDetail(action));
            }

            return details;
        }

        public List<ActionResult> GetActionResults(WPMGMT.BESScraper.Model.Action action)
        {
            List<ActionResult> results = new List<ActionResult>();
            RestClient client = new RestClient(this.BaseURL);
            client.Authenticator = this.Authenticator;

            RestRequest request = new RestRequest("action/{id}/status", Method.GET);
            request.AddUrlSegment("id", action.ActionID.ToString());

            // Execute the request
            XDocument response = Execute(request);

            foreach (XElement computerElement in response.Element("BESAPI").Element("ActionResults").Elements("Computer"))
            {
                DateTime startTime = new DateTime();
                DateTime endTime = new DateTime();

                if (computerElement.Element("StartTime") != null)
                {
                    startTime = Convert.ToDateTime(computerElement.Element("StartTime").Value.ToString());
                }
                if (computerElement.Element("EndTime") != null)
                {
                    endTime = Convert.ToDateTime(computerElement.Element("EndTime").Value.ToString());
                }

                results.Add(new ActionResult(
                                    action.ActionID,                                                            // Action ID
                                    Int32.Parse(computerElement.Attribute("ID").Value.ToString()),              // Computer ID
                                    computerElement.Element("Status").Value.ToString(),                         // Status
                                    Int32.Parse(computerElement.Element("ApplyCount").Value.ToString()),        // Times applied
                                    Int32.Parse(computerElement.Element("RetryCount").Value.ToString()),        // Times retried
                                    Int32.Parse(computerElement.Element("LineNumber").Value.ToString()),        // Which script line is being executed
                                    // Time execution started
                                    (computerElement.Element("StartTime") != null) ? Convert.ToDateTime(computerElement.Element("StartTime").Value.ToString()) : (DateTime?)null,
                                    // Time execution started
                                    (computerElement.Element("EndTime") != null) ? Convert.ToDateTime(computerElement.Element("EndTime").Value.ToString()) : (DateTime?)null
                    ));
            }

            return results;
        }

        public List<ActionResult> GetActionResults(List<WPMGMT.BESScraper.Model.Action> actions)
        {
            List<ActionResult> results = new List<ActionResult>();

            foreach (WPMGMT.BESScraper.Model.Action action in actions)
            {
                results.AddRange(GetActionResults(action));
            }

            return results;
        }

        public List<Analysis> GetAnalyses()
        {
            // The API does not assign an ID to the Site. Therefore, we use the ID assigned by the DB.
            // For this reason we're choosing to get the sites from the DB instead of the REST API here
            BesDb besDb = new BesDb(ConfigurationManager.ConnectionStrings["TEST"].ToString());
            List<Site> sites = besDb.SelectSites();

            List<Analysis> analyses = new List<Analysis>();

            foreach (Site site in sites)
            {
                analyses.AddRange(GetAnalyses(site));
            }

            return analyses;
        }

        public List<Analysis> GetAnalyses(Site site)
        {
            RestClient client = new RestClient(this.BaseURL);
            client.Authenticator = this.Authenticator;

            RestRequest request = new RestRequest("analyses/{sitetype}/{site}", Method.GET);
            request.AddUrlSegment("sitetype", site.Type);
            request.AddUrlSegment("site", site.Name);

            // TODO: Handle master action site properly
            if (site.Type == "master")
            {
                request = new RestRequest("analyses/{sitetype}", Method.GET);
                request.AddUrlSegment("sitetype", site.Type);
            }

            List<Analysis> analyses = Execute<List<Analysis>>(request);

            foreach (Analysis analysis in analyses)
            {
                analysis.SiteID = site.ID;
            }

            return analyses;
        }

        public List<AnalysisProperty> GetAnalysisProperties(List<Analysis> analyses)
        {
            List<AnalysisProperty> properties = new List<AnalysisProperty>();

            foreach (Analysis analysis in analyses)
            {
                properties.AddRange(GetAnalysisProperties(analysis));
            }

            return properties;
        }

        public List<AnalysisProperty> GetAnalysisProperties(Analysis analysis)
        {
            RestClient client = new RestClient(this.BaseURL);
            client.Authenticator = this.Authenticator;

            // The API does not assign an ID to the Site. Therefore, we use the ID assigned by the DB.
            // For this reason we're fetching the list of sites from the DB again, so we can resolve ID->Name
            BesDb besDb = new BesDb(ConfigurationManager.ConnectionStrings["TEST"].ToString());
            Site site = besDb.SelectSite(analysis.SiteID);

            RestRequest request = new RestRequest("analysis/{sitetype}/{site}/{analysisid}", Method.GET);
            request.AddUrlSegment("sitetype", site.Type);
            request.AddUrlSegment("site", site.Name);
            request.AddUrlSegment("analysisid", analysis.AnalysisID.ToString());

            XDocument response = Execute(request);

            List<AnalysisProperty> properties = new List<AnalysisProperty>();

            foreach (XElement propertyElement in response.Element("BES").Element("Analysis").Elements("Property"))
            {
                properties.Add(new AnalysisProperty(analysis.AnalysisID, propertyElement.Attribute("Name").Value.ToString()));
            }

            return properties;
        }

        public List<Computer> GetComputers()
        {
            RestClient client = new RestClient(this.BaseURL);
            client.Authenticator = this.Authenticator;

            RestRequest request = new RestRequest("computers", Method.GET);

            List<Computer> computers = Execute<List<Computer>>(request);

            foreach (Computer computer in computers)
            {
                request = new RestRequest("computer/{id}", Method.GET);
                request.AddUrlSegment("id", computer.ComputerID.ToString());

                XDocument response = Execute(request);
                string hostName = response.Element("BESAPI").Element("Computer").Elements("Property")
                    .Where(e => e.Attribute("Name").Value.ToString() == "Computer Name").Single().Value.ToString();
                computer.ComputerName = hostName;
            }

            return computers;
        }

        public List<ComputerGroup> GetComputerGroups()
        {
            List<ComputerGroup> groups = new List<ComputerGroup>();

            foreach (Site site in GetSites())
            {
                groups.AddRange(GetComputerGroups(site));
            }

            return groups;
        }

        public List<ComputerGroup> GetComputerGroups(Site site)
        {
            List<ComputerGroup> groups = new List<ComputerGroup>();

            RestClient client = new RestClient(this.BaseURL);
            client.Authenticator = this.Authenticator;

            RestRequest request = new RestRequest("computergroups/{sitetype}/{site}", Method.GET);
            request.AddUrlSegment("site", site.Name);
            request.AddUrlSegment("sitetype", site.Type);

            // TODO: Handle master action site properly
            if (site.Type == "master")
            {
                request = new RestRequest("computergroups/{sitetype}", Method.GET);
                request.AddUrlSegment("sitetype", site.Type);
            }

            XDocument response = Execute(request);

            foreach (XElement groupElement in response.Element("BESAPI").Elements("ComputerGroup"))
            {
                groups.Add(GetComputerGroup(site, Int32.Parse(groupElement.Element("ID").Value)));
            }

            return groups;
        }

        public ComputerGroup GetComputerGroup(Site site, int id)
        {
            RestClient client = new RestClient(this.BaseURL);
            client.Authenticator = this.Authenticator;

            RestRequest request = new RestRequest("computergroup/{sitetype}/{site}/{id}", Method.GET);
            request.AddUrlSegment("sitetype", site.Type);
            request.AddUrlSegment("site", site.Name);
            request.AddUrlSegment("id", id.ToString());

            ComputerGroup group = Execute<ComputerGroup>(request);
            group.GroupID = id;

            // The API does not assign an ID to the Site. Therefore, we use the ID assigned by the DB.
            // Let's fetch the Site from the DB first
            BesDb besDb = new BesDb(ConfigurationManager.ConnectionStrings["TEST"].ToString());
            Site dbSite = besDb.SelectSite(site.Name);

            // Assign SiteID if the corresponding Site was found in the DB
            if (dbSite != null)
            {
                group.SiteID = dbSite.ID;
            }

            return group;
        }

        public List<ComputerGroupMember> GetGroupMembers()
        {
            List<ComputerGroupMember> members = new List<ComputerGroupMember>();

            foreach (ComputerGroup group in GetComputerGroups())
            {
                members.AddRange(GetGroupMembers(group));
            }

            return members;
        }

        public List<ComputerGroupMember> GetGroupMembers(ComputerGroup group)
        {
            List<ComputerGroupMember> members = new List<ComputerGroupMember>();

            BesDb besDb = new BesDb(ConfigurationManager.ConnectionStrings["TEST"].ToString());
            Site dbSite = besDb.SelectSite(group.SiteID);

            if ((dbSite != null) && (dbSite.ID != null))
            {
                RestClient client = new RestClient(this.BaseURL);
                client.Authenticator = this.Authenticator;

                RestRequest request = new RestRequest("computergroup/{sitetype}/{site}/{id}/computers", Method.GET);
                request.AddUrlSegment("sitetype", dbSite.Type);
                request.AddUrlSegment("site", dbSite.Name);
                request.AddUrlSegment("id", group.GroupID.ToString());

                XDocument response = Execute(request);

                if (response.Element("BESAPI").Elements("Computer") != null)
                {
                    foreach (XElement computerElement in response.Element("BESAPI").Elements("Computer"))
                    {
                        Uri resourceUri = new Uri(computerElement.Attribute("Resource").Value.ToString());
                        members.Add(new ComputerGroupMember(group.GroupID, Int32.Parse(resourceUri.Segments.Last())));
                    }
                }
            }

            return members;
        }

        public List<Site> GetSites()
        {
            List<Site> sites = new List<Site>();

            RestClient client = new RestClient(this.BaseURL);
            client.Authenticator = this.Authenticator;

            RestRequest request = new RestRequest("sites", Method.GET);

            // Execute the request
            XDocument response = Execute(request);

            // TODO: Handle spaces in URLs correctly
            // TODO: Caps/NoCaps nonsense
            foreach (XElement siteElement in response.Element("BESAPI").Elements())
            {
                if (siteElement.Name.ToString() == "ActionSite")
                {
                    sites.Add(new Site(siteElement.Element("Name").Value.ToString(), "master"));
                }
                else
                {
                    sites.Add(new Site(siteElement.Element("Name").Value.ToString(), siteElement.Name.ToString().Replace("Site", "").ToLower()));
                }
                
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
