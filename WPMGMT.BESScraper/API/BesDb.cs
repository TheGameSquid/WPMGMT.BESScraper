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

        public List<AnalysisProperty> SelectAnalysisProperties()
        {
            return (List<AnalysisProperty>)this.Connection.Query<AnalysisProperty>("SELECT * FROM BESEXT.ANALYSIS_PROPERTY");
        }

        public AnalysisPropertyResult SelectAnalysisPropertyResult(int propertyID, int computerID)
        {
            IEnumerable<AnalysisPropertyResult> results = this.Connection.Query<AnalysisPropertyResult>("SELECT * FROM BESEXT.ANALYSIS_PROPERTY_RESULT WHERE PropertyID = @PropertyID AND ComputerID = @ComputerID", new { PropertyID = propertyID, ComputerID = computerID });
            if (results.Count() > 0)
            {
                return results.Single();
            }
            return null;
        }

        public Baseline SelectBaseline(int baselineID, int siteID)
        {
            IEnumerable<Baseline> baselines = this.Connection.Query<Baseline>("SELECT * FROM BESEXT.BASELINE WHERE BaselineID = @BaselineID AND SiteID = @SiteID", new { BaselineID = baselineID, SiteID = siteID });
            if (baselines.Count() > 0)
            {
                return baselines.Single();
            }
            return null;
        }

        public BaselineResult SelectBaselineResult(int baselineID, int computerID)
        {
            IEnumerable<BaselineResult> results = this.Connection.Query<BaselineResult>("SELECT * FROM BESEXT.BASELINE_RESULT WHERE BaselineID = @BaselineID AND ComputerID = @ComputerID", new { BaselineID = baselineID, ComputerID = computerID });
            if (results.Count() > 0)
            {
                return results.Single();
            }
            return null;
        }

        public List<BaselineResult> SelectBaselineResults()
        {
            return (List<BaselineResult>)this.Connection.Query<BaselineResult>("SELECT * FROM BESEXT.BASELINE_RESULT");
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

        public ComputerGroup SelectComputerGroup(int groupID, int siteID)
        {
            IEnumerable<ComputerGroup> groups = this.Connection.Query<ComputerGroup>("SELECT * FROM BESEXT.\"GROUP\" WHERE GroupID = @GroupID AND @SiteID = SiteID", new { GroupID = groupID, SiteID = siteID });
            if (groups.Count() > 0)
            {
                return groups.Single();
            }
            return null;
        }

        public ComputerGroupMember SelectComputerGroupMember(int groupID, int computerID)
        {
            IEnumerable<ComputerGroupMember> members = this.Connection.Query<ComputerGroupMember>("SELECT * FROM BESEXT.GROUP_MEMBER WHERE GroupID = @GroupID AND @ComputerID = ComputerID", new { GroupID = groupID, ComputerID = computerID });
            if (members.Count() > 0)
            {
                return members.Single();
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
            // TODO: Replace update logic
            this.Connection.Execute("TRUNCATE TABLE BESEXT.ACTION_RESULT");
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

        public void InsertAnalysisPropertyResult(AnalysisPropertyResult result)
        {
            if (SelectAnalysisPropertyResult(result.PropertyID, result.ComputerID) == null)
            {
                Connection.Open();
                int id = Connection.Insert<AnalysisPropertyResult>(result);
                Connection.Close();
            }
            else
            {
                // First, get the existing object
                AnalysisPropertyResult dbResult = SelectAnalysisPropertyResult(result.PropertyID, result.ComputerID);
                // Update the new value
                dbResult.Value = result.Value;
                // Now let's put it back into the DB
                Connection.Update<AnalysisPropertyResult>(dbResult);
            }
        }

        public void InsertAnalysisPropertyResults(List<AnalysisPropertyResult> results)
        {
            // TODO: Replace update logic
            this.Connection.Execute("TRUNCATE TABLE BESEXT.ANALYSIS_PROPERTY_RESULT");
            foreach (AnalysisPropertyResult result in results)
            {
                InsertAnalysisPropertyResult(result);
            }
        }

        public void InsertBaseline(Baseline baseline)
        {
            if (SelectBaseline(baseline.BaselineID, baseline.SiteID) == null)
            {
                Connection.Open();
                int id = Connection.Insert<Baseline>(baseline);
                Connection.Close();
            }
        }

        public void InsertBaselines(List<Baseline> baselines)
        {
            foreach (Baseline baseline in baselines)
            {
                InsertBaseline(baseline);
            }
        }

        public void InsertBaselineResult(BaselineResult result)
        {
            if (SelectBaselineResult(result.BaselineID, result.ComputerID) == null)
            {
                Connection.Open();
                int id = Connection.Insert<BaselineResult>(result);
                Connection.Close();
            }
        }

        public void InsertBaselineResults(List<BaselineResult> results)
        {
            // TODO: Replace update logic
            this.Connection.Execute("TRUNCATE TABLE BESEXT.BASELINE_RESULT");

            foreach (BaselineResult result in results)
            {
                InsertBaselineResult(result);
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

        public void InsertComputerGroup(ComputerGroup group)
        {
            if (SelectComputerGroup(group.GroupID, group.SiteID) == null)
            {
                Connection.Open();
                int id = Connection.Insert<ComputerGroup>(group);
                Connection.Close();
            }
        }

        public void InsertComputerGroups(List<ComputerGroup> groups)
        {
            foreach (ComputerGroup group in groups)
            {
                InsertComputerGroup(group);
            }
        }

        public void InsertComputerGroupMember(ComputerGroupMember member)
        {
            if (SelectComputerGroupMember(member.GroupID, member.ComputerID) == null)
            {
                Connection.Open();
                int id = Connection.Insert<ComputerGroupMember>(member);
                Connection.Close();
            }
        }

        public void InsertComputerGroupMembers(List<ComputerGroupMember> members)
        {
            // TODO: Replace update logic
            this.Connection.Execute("TRUNCATE TABLE BESEXT.GROUP_MEMBER");

            foreach (ComputerGroupMember member in members)
            {
                InsertComputerGroupMember(member);
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