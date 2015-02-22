using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;

using RestSharp;
using Dapper;
using DapperExtensions;

using WPMGMT.BESScraper.Model;
using WPMGMT.BESScraper.API;

namespace WPMGMT.BESScraper
{
    class Program
    {
        // TODO: DateTimeFormat
        static void Main(string[] args)
        {
            BesApi besApi = new BesApi(new Uri("https://DEIMV201.BelgianRail.be:52311/api/"), "iemadmin", "bigfix");
            BesDb besDb = new BesDb(ConfigurationManager.ConnectionStrings["TEST"].ToString());
            //WPMGMT.BESScraper.Model.Action action = besApi.GetAction(1854);
            //Actions actions = besApi.GetActions();
            //ActionDetail detail = besApi.GetActionDetail(1854);

            //WPMGMT.BESScraper.Model.Action action = new Model.Action();
            //action.ID = 1855;
            //action.Name = "TEST POC TROLL";

            //using (SqlConnection cn = new SqlConnection(@"Data Source=10.50.20.128\YPTOSQL002LP;Initial Catalog=YPTO_WPMGMT;Integrated Security=SSPI;"))
            //{
            //    cn.Open();
            //    // TEST
            //    var id = cn.Insert(action);
            //    cn.Close();
            //}
            //using (SqlConnection cn = new SqlConnection(@"Data Source=10.50.20.128\YPTOSQL002LP;Initial Catalog=YPTO_WPMGMT;Integrated Security=SSPI;"))
            //{
            //    cn.Open();
            //    var id = cn.Insert(action);
            //    cn.Close();
            //}

            //List<Computer> computers = besApi.GetComputers();
            //List<Computer> computers = besApi.GetComputers();
            //List<Site> sites = besApi.GetSites();
            //List<Site> sites = besApi.GetSites();
            //besDb.InsertSites(sites);

            //List<ComputerGroup> groups = besApi.GetComputerGroups();

            //List<ComputerGroupMember> members = besApi.GetGroupMembers();

            //List<WPMGMT.BESScraper.Model.Action> actions = besApi.GetActions();
            //besDb.InsertActions(actions);

            //List<ActionDetail> details = new List<ActionDetail>();
            //foreach (WPMGMT.BESScraper.Model.Action action in besApi.GetActions())
            //{
            //    details.Add(besApi.GetActionDetail(action.ActionID));
            //}

            //List<Analysis> analyses = besApi.GetAnalyses();
            //List<AnalysisProperty> properties = besApi.GetAnalysisProperties();

            //List<Computer> computers = besApi.GetComputers();
            //Computer computerLOL = computers.Where(e => e.ComputerID == 13690083).Single();
            //besDb.InsertComputers(computers);

            //List<WPMGMT.BESScraper.Model.Action> actions = besApi.GetActions();
            //besDb.InsertActions(actions);

            //List<ActionDetail> details = besApi.GetActionDetails(actions);
            //besDb.InsertActionDetails(details);

            //List<ActionResult> results = besApi.GetActionResults(actions);
            //besDb.InsertActionResults(results);

            //List<Site> sites = besApi.GetSites();
            //besDb.InsertSites(sites);

            //List<AnalysisProperty> properties = besApi.GetAnalysisProperties(analyses);
            //besDb.InsertAnalysisProperties(properties);

            //List<ComputerGroup> groups = besApi.GetComputerGroups();

            //List<ComputerGroupMember> groupmembers = besApi.GetGroupMembers(groups);

            List<Computer> computers = besApi.GetComputers();
            List<Analysis> analyses = besApi.GetAnalyses();
            //List<AnalysisPropertyResult> results = besApi.GetAnalysisPropertyResults(properties, computers);

            AnalysisProperty property = besDb.SelectAnalysisProperty(961, "SequenceVersion");
            List<AnalysisProperty> properties = new List<AnalysisProperty>();
            properties.Add(property);
            List<AnalysisPropertyResult> results = besApi.GetAnalysisPropertyResults(properties, computers);

            Console.WriteLine("All done :)");
            Console.Read();
        }
    }
}
