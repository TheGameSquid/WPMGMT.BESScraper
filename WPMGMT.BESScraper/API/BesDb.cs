using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using DapperExtensions;
using WPMGMT.BESScraper.Model;

namespace WPMGMT.BESScraper.API
{
    class BesDb
    {
        public BesDb (string ConnectionString)
        {
            this.ConnectionString = ConnectionString;
            this.Connection = new SqlConnection(ConnectionString);
        }

        public SqlConnection Connection     { get; private set; }
        public string ConnectionString      { get; private set; }

        public WPMGMT.BESScraper.Model.Action SelectAction(int actionID)
        {
            IEnumerable<WPMGMT.BESScraper.Model.Action> actions = this.Connection.Query<WPMGMT.BESScraper.Model.Action>("SELECT * FROM BESEXT.ACTION WHERE ActionID = @ActionID", new { ActionID = actionID });
            if (actions.Count() > 0)
            {
                return actions.Single();
            }
            return null;
        }

        public ActionDetail SelectActionDetail(int actionID)
        {
            IEnumerable<ActionDetail> details = this.Connection.Query<ActionDetail>("SELECT * FROM BESEXT.ACTION_DETAIL WHERE ActionID = @ActionID", new { ActionID = actionID });
            if (details.Count() > 0)
            {
                return details.Single();
            }
            return null;
        }

        public ActionResult SelectActionResult(int computerID, int actionID)
        {
            IEnumerable<ActionResult> results = this.Connection.Query<ActionResult>("SELECT * FROM BESEXT.ACTION_RESULT WHERE ActionID = @ActionID AND ComputerID = @ComputerID", new { ActionID = actionID, ComputerID = computerID });
            if (results.Count() > 0)
            {
                return results.Single();
            }
            return null;
        }

        public Analysis SelectAnalysis(int id)
        {
            // BEWARE: This method uses the DB ID, not the API ID!
            IEnumerable<Analysis> analyses = this.Connection.Query<Analysis>("SELECT * FROM BESEXT.ANALYSIS WHERE ID = @ID", new { ID = id });
            if (analyses.Count() > 0)
            {
                return analyses.Single();
            }
            return null;
        }

        public Analysis SelectAnalysis(int siteID, int analysisID)
        {
            IEnumerable<Analysis> analyses = this.Connection.Query<Analysis>("SELECT * FROM BESEXT.ANALYSIS WHERE SiteID = @SiteID AND AnalysisID = @AnalysisID", new { SiteID = siteID, AnalysisID = analysisID });
            if (analyses.Count() > 0)
            {
                return analyses.Single();
            }
            return null;
        }

        public AnalysisProperty SelectAnalysisProperty(int analysisID, string propertyName)
        {
            IEnumerable<AnalysisProperty> properties = this.Connection.Query<AnalysisProperty>("SELECT * FROM BESEXT.ANALYSIS_PROPERTY WHERE AnalysisID = @AnalysisID AND Name = @Name", new { AnalysisID = analysisID, Name = propertyName });
            if (properties.Count() > 0)
            {
                return properties.Single();
            }
            return null;
        }

        public Computer SelectComputer(int computerID)
        {
            IEnumerable<Computer> computers = this.Connection.Query<Computer>("SELECT * FROM BESEXT.COMPUTER WHERE ComputerID = @ComputerID", new { ComputerID = computerID });
            if (computers.Count() > 0)
            {
                return computers.Single();
            }
            return null;
        }

        public Computer SelectComputer(string computerName)
        {
            IEnumerable<Computer> computers = this.Connection.Query<Computer>("SELECT * FROM BESEXT.COMPUTER WHERE ComputerName = @ComputerName", new { ComputerName = computerName });
            if (computers.Count() > 0)
            {
                return computers.Single();
            }
            return null;
        }

        public Site SelectSite(int id)
        {
            IEnumerable<Site> sites = this.Connection.Query<Site>("SELECT * FROM BESEXT.SITE WHERE ID = @Id", new { Id = id });
            if (sites.Count() > 0)
            {
                return sites.Single();
            }
            return null;
        }

        public Site SelectSite(string name)
        {
            IEnumerable<Site> sites = this.Connection.Query<Site>("SELECT * FROM BESEXT.SITE WHERE Name = @Name", new { Name = name });
            if (sites.Count() > 0)
            {
                return sites.Single();
            }
            return null;       
        }

        public List<Site> SelectSites()
        {
            return (List<Site>)this.Connection.Query<Site>("SELECT * FROM BESEXT.SITE");
        }

        public void InsertAction(WPMGMT.BESScraper.Model.Action action)
        {
            if (SelectAction(action.ActionID) == null)
            {
                Connection.Open();
                int id = Connection.Insert<WPMGMT.BESScraper.Model.Action>(action);
                Connection.Close();
            }
        }

        public void InsertActions(List<WPMGMT.BESScraper.Model.Action> actions)
        {
            foreach (WPMGMT.BESScraper.Model.Action action in actions)
            {
                InsertAction(action);
            }
        }

        public void InsertActionDetail(ActionDetail detail)
        {
            if (SelectActionDetail(detail.ActionID) == null)
            {
                Connection.Open();
                int id = Connection.Insert<ActionDetail>(detail);
                Connection.Close();
            }
        }

        public void InsertActionDetails(List<ActionDetail> details)
        {
            foreach (ActionDetail detail in details)
            {
                InsertActionDetail(detail);
            }
        }

        public void InsertActionResult(ActionResult result)
        {
            if (SelectActionResult(result.ComputerID, result.ActionID) == null)
            {
                Connection.Open();
                int id = Connection.Insert<ActionResult>(result);
                Connection.Close();
            }
        }

        public void InsertActionResults(List<ActionResult> results)
        {
            foreach (ActionResult result in results)
            {
                InsertActionResult(result);
            }
        }

        public void InsertAnalysis(Analysis analysis)
        {
            if (SelectAnalysis(analysis.SiteID, analysis.AnalysisID) == null)
            {
                Connection.Open();
                int id = Connection.Insert<Analysis>(analysis);
                Connection.Close();
            }
        }

        public void InsertAnalyses(List<Analysis> analyses)
        {
            foreach (Analysis analysis in analyses)
            {
                InsertAnalysis(analysis);
            }
        }

        public void InsertAnalysisProperty(AnalysisProperty property)
        {
            if (SelectAnalysisProperty(property.AnalysisID, property.Name) == null)
            {
                Connection.Open();
                int id = Connection.Insert<AnalysisProperty>(property);
                Connection.Close();
            }
        }

        public void InsertAnalysisProperties(List<AnalysisProperty> properties)
        {
            foreach (AnalysisProperty property in properties)
            {
                InsertAnalysisProperty(property);
            }
        }

        public void InsertComputer(Computer computer)
        {
            if (SelectComputer(computer.ComputerID) == null)
            {
                Connection.Open();
                int id = Connection.Insert<Computer>(computer);
                Connection.Close();
            }
        }

        public void InsertComputers(List<Computer> computers)
        {
            foreach (Computer computer in computers)
            {
                InsertComputer(computer);
            }
        }

        public void InsertSite(Site site)
        {
            if (SelectSite(site.Name) == null)
            {
                Connection.Open();
                int id = Connection.Insert<Site>(site);
                Connection.Close();
            }
        }

        public void InsertSites(List<Site> sites)
        {
            foreach (Site site in sites)
            {
                InsertSite(site);
            }
        }
    }
}