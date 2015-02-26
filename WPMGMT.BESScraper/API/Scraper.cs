using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using WPMGMT.BESScraper.Model;
using Dapper;
using DapperExtensions;
using Quartz;
using NLog;

namespace WPMGMT.BESScraper.API
{
    class Scraper : IJob
    {
        public Scraper()
        {
            // Empty constructor for Quartz


            this.API = new BesApi(
                                    ConfigurationManager.AppSettings["ApiEndpoint"],
                                    ConfigurationManager.AppSettings["ApiUser"],
                                    ConfigurationManager.AppSettings["ApiPassword"]
                                 );
            this.DB = new BesDb(ConfigurationManager.ConnectionStrings["DB"].ToString());
            this.Logger = LogManager.GetCurrentClassLogger();
        }

        public Scraper (BesApi api, BesDb db)
        {
            this.API = api;
            this.DB = db;
            this.Logger = LogManager.GetCurrentClassLogger();
        }

        public BesApi API       { get; private set; }
        public BesDb DB         { get; private set; }
        public Logger Logger    { get; private set; }

        //public void Execute(IJobExecutionContext context)
        //{
        //    //Logger.Warn("Gepoet!");
        //    Console.WriteLine("HAHAHAHAH");
        //}

        //public void Run()
        public void Execute(IJobExecutionContext context)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();

            Logger.Info("Scraper Run() initiated");
            try
            {
                // Step 1: Fetch/Submit all Site objects
                this.ScrapeSites();
                // Step 2: Fetch/Submit all Computer objects
                this.ScrapeComputers();
                // Step 3: Fetch/Submit all ComputerGroup objects
                this.ScrapeComputerGroups();
                // Step 4: Fetch/Submit all ComputerGroupMember objects
                this.ScrapeComputerGroups();
                // Step 4: Fetch/Submit all Baseline objects
                this.ScrapeBaselines();
                // Step 5: Fetch/Submit all BaselineResult objects
                this.ScrapeBaselineResults();
                // Step 6: Fetch/Submit all Action objects
                this.ScrapeActions();
                // Step 7: Fetch/Submit all ActionDetail objects
                this.ScrapeActionDetails();
                // Step 8: Fetch/Submit all ActionResult objects
                this.ScrapeActionResults();
                // Step 9: Fetch/Submit all Analysis objects
                this.ScrapeAnalyses();
                // Step 10: Fetch/Submit all AnalysisProperty objects
                this.ScrapeAnalysisProperties();
                // Step 11: Fetch/Submit all AnalysisPropertyResult objects
                this.ScrapeAnalysisPropertyResults();

                timer.Stop();
                Logger.Info("Finished Run() in {0}", timer.Elapsed);
            }
            catch (Exception e)
            {
                Logger.ErrorException("Exception encountered during Scraper.Run()", e);
            }
        }

        // Scrapes all Sites through /api/sites
        private void ScrapeSites()
        {
            Logger.Info("ScrapeSites() started");

            // Retrieve all Sites from the API
            List<Site> apiSites = API.GetSites();   
            // Retrieve all Sites in the DB
            IEnumerable<Site> dbSites = DB.Connection.Query<Site>(@"SELECT * FROM [BESEXT].[SITE]");

            foreach (Site apiSite in apiSites)
            {
                // Check if the Site already exists in the DB
                if (!dbSites.Any(s => s.Name == apiSite.Name))
                {
                    Logger.Debug("Inserting Site: {0}", apiSite.Name);
                    DB.Connection.Insert<Site>(apiSite);
                }
                else
                {
                    // Else update the Site
                    Site dbSite = dbSites.Where(s => s.Name == apiSite.Name).Single();
                    dbSite.Type = apiSite.Type;

                    Logger.Debug("Updating Site: {0}", dbSite.Name);
                    if (!DB.Connection.Update<Site>(dbSite))
                    {
                        Exception e = new Exception("Unable to UPDATE Site object");
                        e.Data["ObjectData"] = dbSite;
                        throw e;
                    }
                }
            }

            foreach (Site dbSite in dbSites)
            {
                // If the Site in the db could not be retrieved using the API, delete it
                if (!apiSites.Any(s => s.Name == dbSite.Name))
                {
                    Logger.Debug("Deleting Site: {0}", dbSite.Name);
                    if (!DB.Connection.Delete(dbSite))
                    {
                        Exception e = new Exception("Unable to DELETE Site object");
                        e.Data["ObjectData"] = dbSite;
                        throw e;
                    }
                }
            }
        }

        // Scrapes all Computers through /api/computers
        private void ScrapeComputers()
        {
            Logger.Info("ScrapeComputers() started");

            // Retrieve all Computers from the API
            List<Computer> apiComputers = API.GetComputers();
            // Retrieve all Computers in the DB
            IEnumerable<Computer> dbComputers = DB.Connection.Query<Computer>(@"SELECT * FROM [BESEXT].[COMPUTER]");

            foreach (Computer apiComputer in apiComputers)
            {
                // Check if the Computer already exists in the DB
                if (!dbComputers.Any(c => c.ComputerID == apiComputer.ComputerID))
                {
                    Logger.Debug("Inserting Computer: {0}", apiComputer.ComputerID);
                    DB.Connection.Insert<Computer>(apiComputer);
                }
                else
                {
                    // Else update the Site
                    Computer dbComputer = dbComputers.Where(c => c.ComputerID == apiComputer.ComputerID).Single();
                    dbComputer.LastReportTime = apiComputer.LastReportTime;

                    Logger.Debug("Updating Computer: {0}", dbComputer.ComputerID);
                    if (!DB.Connection.Update<Computer>(dbComputer))
                    {
                        Exception e = new Exception("Unable to UPDATE Computer object");
                        e.Data["ObjectData"] = dbComputer;
                        throw e;
                    }
                }
            }

            foreach (Computer dbComputer in dbComputers)
            {
                // If the Computer in the db could not be retrieved using the API, delete it
                if (!apiComputers.Any(c => c.ComputerID == dbComputer.ComputerID))
                {
                    Logger.Debug("Deleting Computer: {0}", dbComputer.ComputerID);
                    if (!DB.Connection.Delete(dbComputer))
                    {
                        Exception e = new Exception("Unable to DELETE Computer object");
                        e.Data["ObjectData"] = dbComputer;
                        throw e;
                    }
                }
            }
        }

        // Scrapes all ComputerGroups through /api/computergroups/{sitetype}/{site}
        private void ScrapeComputerGroups()
        {
            Logger.Info("ScrapeComputerGroups() started");

            // Retrieve all ComputerGroups from the API
            List<ComputerGroup> apiComputerGroups = API.GetComputerGroups();
            // Retrieve all ComputerGroups in the DB
            IEnumerable<ComputerGroup> dbComputerGroups = DB.Connection.Query<ComputerGroup>(@"SELECT * FROM [BESEXT].[GROUP]");

            foreach (ComputerGroup apiComputerGroup in apiComputerGroups)
            {
                // Check if the ComputerGroup already exists in the DB
                if (!dbComputerGroups.Any(g => g.GroupID == apiComputerGroup.GroupID))
                {
                    Logger.Debug("Inserting ComputerGroup: {0}", apiComputerGroup.GroupID);
                    DB.Connection.Insert<ComputerGroup>(apiComputerGroup);
                }
                else
                {
                    // Else update the ComputerGroup
                    ComputerGroup dbComputerGroup = dbComputerGroups.Where(g => g.GroupID == apiComputerGroup.GroupID).Single();
                    dbComputerGroup.Name = apiComputerGroup.Name;
                    dbComputerGroup.SiteID = apiComputerGroup.SiteID;

                    Logger.Debug("Updating ComputerGroup: {0}", dbComputerGroup.GroupID);
                    if (!DB.Connection.Update<ComputerGroup>(dbComputerGroup))
                    {
                        Exception e = new Exception("Unable to UPDATE ComputerGroup object");
                        e.Data["ObjectData"] = dbComputerGroup;
                        throw e;
                    }
                }
            }

            foreach (ComputerGroup dbComputerGroup in dbComputerGroups)
            {
                // If the ComputerGroup in the db could not be retrieved using the API, delete it
                if (!apiComputerGroups.Any(g => g.GroupID == dbComputerGroup.GroupID))
                {
                    Logger.Debug("Deleting ComputerGroup: {0}", dbComputerGroup.GroupID);
                    if (!DB.Connection.Delete(dbComputerGroup))
                    {
                        Exception e = new Exception("Unable to DELETE ComputerGroup object");
                        e.Data["ObjectData"] = dbComputerGroup;
                        throw e;
                    }
                }
            }
        }

        // Scrapes all ComputerGroupMembers through /api/computergroup/{sitetype}/{site}/{id}/computers
        private void ScrapeComputerGroupMembers()
        {
            Logger.Info("ScrapeComputerGroupMembers() started");

            // Get a collection of ComputerGroups from the database
            List<ComputerGroup> dbComputerGroups = DB.Connection.Query<ComputerGroup>("SELECT * FROM [BESEXT].[GROUP]").ToList();

            // Retrieve all ComputerGroupMembers from the API
            List<ComputerGroupMember> apiComputerGroupMembers = API.GetGroupMembers(dbComputerGroups);
            // Retrieve all ComputerGroupMembers in the DB
            IEnumerable<ComputerGroupMember> dbComputerGroupMembers = DB.Connection.Query<ComputerGroupMember>(@"SELECT * FROM BESEXT.GROUP_MEMBER");

            foreach (ComputerGroupMember apiComputerGroupMember in apiComputerGroupMembers)
            {
                // Check if the ComputerGroupMember already exists in the DB
                if (!dbComputerGroupMembers.Any(gm => gm.GroupID == apiComputerGroupMember.GroupID && gm.ComputerID == apiComputerGroupMember.ComputerID))
                {
                    Logger.Debug("Inserting ComputerGroupMember: {0}:{1}", apiComputerGroupMember.GroupID, apiComputerGroupMember.ComputerID);
                    DB.Connection.Insert<ComputerGroupMember>(apiComputerGroupMember);
                }
                else
                {
                    // There's nothing to update :)
                }
            }

            foreach (ComputerGroupMember dbComputerGroupMember in dbComputerGroupMembers)
            {
                // If the ComputerGroup in the db could not be retrieved using the API, delete it
                if (!apiComputerGroupMembers.Any(gm => gm.GroupID == dbComputerGroupMember.GroupID && gm.ComputerID == dbComputerGroupMember.ComputerID))
                {
                    Logger.Debug("Deleting ComputerGroupMember: {{0}:{1}", dbComputerGroupMember.GroupID, dbComputerGroupMember.ComputerID);
                    if (!DB.Connection.Delete(dbComputerGroupMember))
                    {
                        Exception e = new Exception("Unable to DELETE ComputerGroupMember object");
                        e.Data["ObjectData"] = dbComputerGroupMember;
                        throw e;
                    }
                }
            }
        }

        // Scrapes all Baselines through /api/site/{sitetype}/{site}/content
        private void ScrapeBaselines()
        {
            Logger.Info("ScrapeBaselines() started");

            // Get all available Sites from the DB
            List<Site> dbSites = (List<Site>)DB.Connection.Query<Site>(@"SELECT * FROM [BESEXT].[SITE]");

            // Retrieve all Baselines from the API
            List<Baseline> apiBaselines = API.GetBaselines(dbSites);
            // Retrieve all Baselines in the DB
            IEnumerable<Baseline> dbBaselines = DB.Connection.Query<Baseline>(@"SELECT * FROM [BESEXT].[BASELINE]");

            foreach (Baseline apiBaseline in apiBaselines)
            {
                // Check if the Baseline already exists in the DB
                if (!dbBaselines.Any(b => b.BaselineID == apiBaseline.BaselineID))
                {
                    Logger.Debug("Inserting Baseline: {0}", apiBaseline.BaselineID);
                    DB.Connection.Insert<Baseline>(apiBaseline);
                }
                else
                {
                    // Else update the Baseline
                    Baseline dbBaseline = dbBaselines.Where(b => b.BaselineID == apiBaseline.BaselineID).Single();
                    dbBaseline.SiteID = apiBaseline.SiteID;
                    dbBaseline.Name = apiBaseline.Name;

                    Logger.Debug("Updating Baseline: {0}", dbBaseline.BaselineID);
                    if (!DB.Connection.Update<Baseline>(dbBaseline))
                    {
                        Exception e = new Exception("Unable to UPDATE Baseline object");
                        e.Data["ObjectData"] = dbBaseline;
                        throw e;
                    }
                }
            }

            foreach (Baseline dbBaseline in dbBaselines)
            {
                // If the Baseline in the db could not be retrieved using the API, delete it
                if (!apiBaselines.Any(b => b.BaselineID == dbBaseline.BaselineID))
                {
                    Logger.Debug("Deleting Baseline: {0}", dbBaseline.BaselineID);
                    if (!DB.Connection.Delete(dbBaseline))
                    {
                        Exception e = new Exception("Unable to DELETE Baseline object");
                        e.Data["ObjectData"] = dbBaseline;
                        throw e;
                    }
                }
            }
        }

        // Scrapes all BaselineResults through /api/fixlet/{sitetype}/{site}/{baselineid}/computers
        private void ScrapeBaselineResults()
        {
            Logger.Info("ScrapeBaselineResults() started");

            // Get all available Baselines from the DB
            List<Baseline> dbBaselines = (List<Baseline>)DB.Connection.Query<Baseline>(@"SELECT * FROM [BESEXT].[BASELINE]");

            // Retrieve all BaselineResults from the API
            List<BaselineResult> apiBaselineResults = API.GetBaselineResults(dbBaselines);
            // Retrieve all BaselineResults in the DB
            IEnumerable<BaselineResult> dbBaselineResults = DB.Connection.Query<BaselineResult>(@"SELECT * FROM [BESEXT].[BASELINE_RESULT]");

            foreach (BaselineResult apiBaselineResult in apiBaselineResults)
            {
                // Check if the BaselineResult already exists in the DB
                if (!dbBaselineResults.Any(r => r.BaselineID == apiBaselineResult.BaselineID && r.ComputerID == apiBaselineResult.ComputerID))
                {
                    Logger.Debug("Inserting BaselineResult: {0}:{1}", apiBaselineResult.BaselineID, apiBaselineResult.ComputerID);
                    DB.Connection.Insert<BaselineResult>(apiBaselineResult);
                }
                else
                {
                    // Nothing to update
                }
            }

            foreach (BaselineResult dbBaselineResult in dbBaselineResults)
            {
                // If the BaselineResult in the db could not be retrieved using the API, delete it
                if (!apiBaselineResults.Any(r => r.BaselineID == dbBaselineResult.BaselineID && r.ComputerID == dbBaselineResult.ComputerID))
                {
                    Logger.Debug("Deleting BaselineResult: {0}:{1}", dbBaselineResult.BaselineID, dbBaselineResult.ComputerID);
                    if (!DB.Connection.Delete(dbBaselineResult))
                    {
                        Exception e = new Exception("Unable to DELETE BaselineResult object");
                        e.Data["ObjectData"] = dbBaselineResult;
                        throw e;
                    }
                }
            }
        }

        // Scrapes all Actions through /api/actions
        private void ScrapeActions()
        {
            Logger.Info("ScrapeActions() started");

            // Retrieve all Actions from the API
            List<Model.Action> apiActions = API.GetActions();
            // Retrieve all Actions in the DB
            IEnumerable<Model.Action> dbActions = DB.Connection.Query<Model.Action>(@"SELECT * FROM [BESEXT].[ACTION]");

            foreach (Model.Action apiAction in apiActions)
            {
                // Check if the Action already exists in the DB
                if (!dbActions.Any(a => a.ActionID == apiAction.ActionID))
                {
                    Logger.Debug("Inserting Action: {0}", apiAction.ActionID);
                    DB.Connection.Insert<Model.Action>(apiAction);
                }
                else
                {
                    // Else update the Action
                    Model.Action dbAction = dbActions.Where(a => a.ActionID == apiAction.ActionID).Single();
                    dbAction.Name = apiAction.Name;

                    Logger.Debug("Updating Action: {0}", dbAction.ActionID);
                    if (!DB.Connection.Update<Model.Action>(dbAction))
                    {
                        Exception e = new Exception("Unable to UPDATE Action object");
                        e.Data["ObjectData"] = dbAction;
                        throw e;
                    }
                }
            }

            foreach (Model.Action dbAction in dbActions)
            {
                // If the Action in the db could not be retrieved using the API, delete it
                if (!apiActions.Any(a => a.ActionID == dbAction.ActionID))
                {
                    Logger.Debug("Deleting Action: {0}", dbAction.ActionID);
                    if (!DB.Connection.Delete(dbAction))
                    {
                        Exception e = new Exception("Unable to DELETE Action object");
                        e.Data["ObjectData"] = dbAction;
                        throw e;
                    }
                }
            }
        }

        // Scrapes all ActionsDetails through /api/action/{id}/status"
        private void ScrapeActionDetails()
        {
            Logger.Info("ScrapeActionDetails() started");

            // Get all available Actions from the DB
            List<Model.Action> dbActions = (List<Model.Action>)DB.Connection.Query<Model.Action>(@"SELECT * FROM [BESEXT].[ACTION]");

            // Retrieve all ActionDetails from the API
            List<ActionDetail> apiActionDetails = API.GetActionDetails(dbActions);
            // Retrieve all ActionDetails in the DB
            IEnumerable<ActionDetail> dbActionDetails = DB.Connection.Query<ActionDetail>(@"SELECT * FROM [BESEXT].[ACTION_DETAIL]");

            foreach (ActionDetail apiActionDetail in apiActionDetails)
            {
                // Check if the ActionDetail already exists in the DB
                if (!dbActionDetails.Any(ad => ad.ActionID == apiActionDetail.ActionID))
                {
                    Logger.Debug("Inserting ActionDetail: {0}", apiActionDetail.ActionID);
                    DB.Connection.Insert<ActionDetail>(apiActionDetail);
                }
                else
                {
                    // Else update the ActionDetail
                    ActionDetail dbActionDetail = dbActionDetails.Where(ad => ad.ActionID == apiActionDetail.ActionID).Single();
                    dbActionDetail.Status = apiActionDetail.Status;
                    dbActionDetail.DateIssued = apiActionDetail.DateIssued;

                    Logger.Debug("Updating ActionDetail: {0}", dbActionDetail.ID);
                    if (!DB.Connection.Update<ActionDetail>(dbActionDetail))
                    {
                        Exception e = new Exception("Unable to UPDATE ActionDetail object");
                        e.Data["ObjectData"] = dbActionDetail;
                        throw e;
                    }
                }
            }

            foreach (ActionDetail dbActionDetail in dbActionDetails)
            {
                // If the ActionDetail in the db could not be retrieved using the API, delete it
                if (!apiActionDetails.Any(ad => ad.ActionID == dbActionDetail.ActionID))
                {
                    Logger.Debug("Deleting ActionDetail: {0}", dbActionDetail.ID);
                    if (!DB.Connection.Delete(dbActionDetail))
                    {
                        Exception e = new Exception("Unable to DELETE ActionDetail object");
                        e.Data["ObjectData"] = dbActionDetail;
                        throw e;
                    }
                }
            }
        }

        // Scrapes all ActionsResults through /api/action/{id}/status"
        private void ScrapeActionResults()
        {
            Logger.Info("ScrapeActionResults() started");

            // Get all available Actions from the DB
            List<Model.Action> dbActions = (List<Model.Action>)DB.Connection.Query<Model.Action>(@"SELECT * FROM [BESEXT].[ACTION]");

            // Retrieve all ActionResults from the API
            List<ActionResult> apiActionResults = API.GetActionResults(dbActions);
            // Retrieve all ActionResults in the DB
            IEnumerable<ActionResult> dbActionResults = DB.Connection.Query<ActionResult>(@"SELECT * FROM [BESEXT].[ACTION_RESULT]");

            foreach (ActionResult apiActionResult in apiActionResults)
            {
                // Check if the ActionResult already exists in the DB
                if (!dbActionResults.Any(ar => ar.ActionID == apiActionResult.ActionID && ar.ComputerID == apiActionResult.ComputerID))
                {
                    Logger.Debug("Inserting ActionResult: {0}:{1}", apiActionResult.ActionID, apiActionResult.ComputerID);
                    DB.Connection.Insert<ActionResult>(apiActionResult);
                }
                else
                {
                    // Else update the ActionResult
                    ActionResult dbActionResult = dbActionResults.Where(ar => ar.ActionID == apiActionResult.ActionID && ar.ComputerID == apiActionResult.ComputerID).Single();
                    dbActionResult.ApplyCount = apiActionResult.ApplyCount;
                    dbActionResult.EndTime = apiActionResult.EndTime;
                    dbActionResult.RetryCount = apiActionResult.RetryCount;
                    dbActionResult.StartTime = apiActionResult.StartTime;
                    dbActionResult.State = apiActionResult.State;
                    dbActionResult.Status = apiActionResult.Status;

                    Logger.Debug("Updating ActionResult: {0}:{1}", apiActionResult.ActionID, apiActionResult.ComputerID);
                    if (!DB.Connection.Update<ActionResult>(dbActionResult))
                    {
                        Exception e = new Exception("Unable to UPDATE ActionResult object");
                        e.Data["ObjectData"] = dbActionResult;
                        throw e;
                    }
                }
            }

            foreach (ActionResult dbActionResult in dbActionResults)
            {
                // If the ActionResult in the db could not be retrieved using the API, delete it
                if (!apiActionResults.Any(ar => ar.ActionID == dbActionResult.ActionID && ar.ComputerID == dbActionResult.ComputerID))
                {
                    Logger.Debug("Deleting ActionResult: {0}", dbActionResult.ID);
                    if (!DB.Connection.Delete(dbActionResult))
                    {
                        Exception e = new Exception("Unable to DELETE ActionResult object");
                        e.Data["ObjectData"] = dbActionResult;
                        throw e;
                    }
                }
            }
        }

        // Scrapes all Analyses through /api/analyses/{sitetype}/{site}"
        private void ScrapeAnalyses()
        {
            Logger.Info("ScrapeAnalyses() started");

            // Get all available Sites from the DB
            List<Site> dbSites = (List<Site>)DB.Connection.Query<Site>(@"SELECT * FROM [BESEXT].[SITE]");

            // Retrieve all Analyses from the API
            List<Analysis> apiAnalyses = API.GetAnalyses(dbSites);
            // Retrieve all Analyses in the DB
            IEnumerable<Analysis> dbAnalyses = DB.Connection.Query<Analysis>(@"SELECT * FROM [BESEXT].[ANALYSIS]");

            foreach (Analysis apiAnalysis in apiAnalyses)
            {
                // Check if the Analysis already exists in the DB
                if (!dbAnalyses.Any(a => a.AnalysisID == apiAnalysis.AnalysisID && a.SiteID == apiAnalysis.SiteID))
                {
                    Logger.Debug("Inserting Analysis: {0}:{1}", apiAnalysis.AnalysisID, apiAnalysis.SiteID);
                    DB.Connection.Insert<Analysis>(apiAnalysis);
                }
                else
                {
                    // Else update the Analysis
                    Analysis dbAnalysis = dbAnalyses.Where(a => a.AnalysisID == apiAnalysis.AnalysisID && a.SiteID == apiAnalysis.SiteID).Single();
                    dbAnalysis.Name = apiAnalysis.Name;

                    Logger.Debug("Updating Analysis: {0}:{1}", apiAnalysis.AnalysisID, apiAnalysis.SiteID);
                    if (!DB.Connection.Update<Analysis>(dbAnalysis))
                    {
                        Exception e = new Exception("Unable to UPDATE Analysis object");
                        e.Data["ObjectData"] = dbAnalysis;
                        throw e;
                    }
                }
            }

            foreach (Analysis dbAnalysis in dbAnalyses)
            {
                // If the Analysis in the db could not be retrieved using the API, delete it
                if (!apiAnalyses.Any(a => a.AnalysisID == dbAnalysis.AnalysisID && a.SiteID == dbAnalysis.SiteID))
                {
                    Logger.Debug("Deleting Analysis: {0}", dbAnalysis.ID);
                    if (!DB.Connection.Delete(dbAnalysis))
                    {
                        Exception e = new Exception("Unable to DELETE Analysis object");
                        e.Data["ObjectData"] = dbAnalysis;
                        throw e;
                    }
                }
            }
        }

        // Scrapes all AnalysisProperties through /api/analysis/{sitetype}/{site}/{analysisid}"
        private void ScrapeAnalysisProperties()
        {
            Logger.Info("ScrapeAnalysisProperties() started");

            // Get all available Analyses from the DB
            List<Analysis> dbAnalyses = (List<Analysis>)DB.Connection.Query<Analysis>(@"SELECT * FROM [BESEXT].[ANALYSIS]");

            // Retrieve all AnalysisProperties from the API
            List<AnalysisProperty> apiAnalysisProperties = API.GetAnalysisProperties(dbAnalyses);
            // Retrieve all AnalysisProperties in the DB
            IEnumerable<AnalysisProperty> dbAnalysisProperties = DB.Connection.Query<AnalysisProperty>(@"SELECT * FROM [BESEXT].[ANALYSIS_PROPERTY]");

            foreach (AnalysisProperty apiAnalysisProperty in apiAnalysisProperties)
            {
                // Check if the AnalysisProperty already exists in the DB
                if (!dbAnalysisProperties.Any(ap => ap.AnalysisID == apiAnalysisProperty.AnalysisID 
                                                && ap.SequenceNo == apiAnalysisProperty.SequenceNo))
                {
                    Logger.Debug("Inserting AnalysisProperty: {0}:{1}", apiAnalysisProperty.AnalysisID, apiAnalysisProperty.SequenceNo);
                    DB.Connection.Insert<AnalysisProperty>(apiAnalysisProperty);
                }
                else
                {
                    // Else update the AnalysisProperty
                    AnalysisProperty dbAnalysisProperty = dbAnalysisProperties.Where(ap => ap.AnalysisID == apiAnalysisProperty.AnalysisID
                                                && ap.SequenceNo == apiAnalysisProperty.SequenceNo).Single();
                    dbAnalysisProperty.Name = apiAnalysisProperty.Name;

                    Logger.Debug("Updating AnalysisProperty: {0}:{1}", apiAnalysisProperty.AnalysisID, apiAnalysisProperty.SequenceNo);
                    if (!DB.Connection.Update<AnalysisProperty>(dbAnalysisProperty))
                    {
                        Exception e = new Exception("Unable to UPDATE AnalysisProperty object");
                        e.Data["ObjectData"] = dbAnalysisProperty;
                        throw e;
                    }
                }
            }

            foreach (AnalysisProperty dbAnalysisProperty in dbAnalysisProperties)
            {
                // If the AnalysisProperty in the db could not be retrieved using the API, delete it
                if (!dbAnalysisProperties.Any(ap => ap.AnalysisID == dbAnalysisProperty.AnalysisID 
                                                && ap.SequenceNo == dbAnalysisProperty.SequenceNo))
                {
                    Logger.Debug("Deleting AnalysisProperty: {0}", dbAnalysisProperty.ID);
                    if (!DB.Connection.Delete(dbAnalysisProperty))
                    {
                        Exception e = new Exception("Unable to DELETE AnalysisProperty object");
                        e.Data["ObjectData"] = dbAnalysisProperty;
                        throw e;
                    }
                }
            }
        }

        // Scrapes all AnalysisPropertyResults through the use of relevance :("
        private void ScrapeAnalysisPropertyResults()
        {
            Logger.Info("ScrapeAnalysisPropertyResults() started");

            // Get all available AnalysisProperties from the DB
            List<AnalysisProperty> dbAnalysisProperties = (List<AnalysisProperty>)DB.Connection.Query<AnalysisProperty>(@"SELECT * FROM [BESEXT].[ANALYSIS_PROPERTY]");

            // Retrieve all AnalysisPropertyResults from the API
            List<AnalysisPropertyResult> apiAnalysisPropertyResults = API.GetAnalysisPropertyResults(dbAnalysisProperties);
            // Retrieve all AnalysisPropertyResults in the DB
            IEnumerable<AnalysisPropertyResult> dbAnalysisPropertyResults = DB.Connection.Query<AnalysisPropertyResult>(@"SELECT * FROM [BESEXT].[ANALYSIS_PROPERTY_RESULT]");

            foreach (AnalysisPropertyResult apiAnalysisPropertyResult in apiAnalysisPropertyResults)
            {
                // Check if the AnalysisPropertyResult already exists in the DB
                if (!dbAnalysisPropertyResults.Any(ar => ar.PropertyID == apiAnalysisPropertyResult.PropertyID
                                                && ar.ComputerID == apiAnalysisPropertyResult.ComputerID))
                {
                    Logger.Debug("Inserting AnalysisPropertyResult: {0}:{1}", apiAnalysisPropertyResult.PropertyID, apiAnalysisPropertyResult.ComputerID);
                    DB.Connection.Insert<AnalysisPropertyResult>(apiAnalysisPropertyResult);
                }
                else
                {
                    // Else update the AnalysisPropertyResult
                    AnalysisPropertyResult dbAnalysisPropertyResult = dbAnalysisPropertyResults.Where(ar => ar.PropertyID == apiAnalysisPropertyResult.PropertyID
                                                && ar.ComputerID == apiAnalysisPropertyResult.ComputerID).Single();
                    dbAnalysisPropertyResult.Value = apiAnalysisPropertyResult.Value;

                    Logger.Debug("Updating AnalysisPropertyResult: {0}:{1}", apiAnalysisPropertyResult.PropertyID, apiAnalysisPropertyResult.ComputerID);
                    if (!DB.Connection.Update<AnalysisPropertyResult>(dbAnalysisPropertyResult))
                    {
                        Exception e = new Exception("Unable to UPDATE AnalysisPropertyResult object");
                        e.Data["ObjectData"] = dbAnalysisPropertyResult;
                        throw e;
                    }
                }
            }

            foreach (AnalysisPropertyResult dbAnalysisPropertyResult in dbAnalysisPropertyResults)
            {
                // If the AnalysisPropertyResult in the db could not be retrieved using the API, delete it
                if (!dbAnalysisPropertyResults.Any(ar => ar.PropertyID == dbAnalysisPropertyResult.PropertyID
                                                && ar.ComputerID == dbAnalysisPropertyResult.ComputerID))
                {
                    Logger.Debug("Deleting AnalysisPropertyResult: {0}:{1}", dbAnalysisPropertyResult.PropertyID, dbAnalysisPropertyResult.ComputerID);
                    if (!DB.Connection.Delete(dbAnalysisPropertyResult))
                    {
                        Exception e = new Exception("Unable to DELETE AnalysisPropertyResult object");
                        e.Data["ObjectData"] = dbAnalysisPropertyResult;
                        throw e;
                    }
                }
            }
        }
    }
}
