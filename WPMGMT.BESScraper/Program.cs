using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
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

            //List<Computer> computers = besApi.GetComputers();
            //besDb.InsertComputers(computers);
            //List<Analysis> analyses = besApi.GetAnalyses();
            //besDb.InsertAnalyses(analyses);
            //List<AnalysisProperty> properties = besApi.GetAnalysisProperties(analyses);
            //besDb.InsertAnalysisProperties(properties);
            //List<AnalysisProperty> properties = besDb.SelectAnalysisProperties();
            
            //Stopwatch timer = new Stopwatch();

            //timer.Start();
            //List<AnalysisPropertyResult> results = besApi.GetAnalysisPropertyResults(properties);
            //timer.Stop();

            //Console.WriteLine("Time elapsed: {0}", timer.Elapsed);

            //besDb.InsertAnalysisPropertyResults(results);

            List<Site> sites = besDb.SelectSites();
            List<Baseline> baselines = besApi.GetBaselines(sites);
            besDb.InsertBaselines(baselines);

            List<BaselineResult> results = besApi.GetBaselineResults(baselines);
            besDb.InsertBaselineResults(results);

            BaselineResult result1 = new BaselineResult(1, 1);
            BaselineResult result2 = new BaselineResult(1, 1);
            BaselineResult result3 = new BaselineResult(2, 2);

            List<BaselineResult> list1 = new List<BaselineResult>();
            list1.Add(result1);

            List<BaselineResult> list2 = new List<BaselineResult>();
            list2.Add(result1);
            list2.Add(result1);

            //Console.WriteLine("Resul1 and Result2: {0}", result1.Equals(result2));
            //Console.WriteLine("Resul1 and Result3: {0}", result1.Equals(result3));

            var resultssssss = list1.Where(api => !list2.Any(db => api.BaselineID == db.BaselineID));
            list1.Intersect(list2);

            Console.WriteLine("All done :)");
            Console.Read();
        }
    }
}