using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WPMGMT.BESScraper.Model;
using Dapper;
using DapperExtensions;

namespace WPMGMT.BESScraper.API
{
    class Scraper
    {
        public Scraper (BesApi api, BesDb db)
        {
            this.API = api;
            this.DB = db;
        }

        public BesApi API   { get; private set; }
        public BesDb DB     { get; private set; }

        public void Run()
        {
            // Step 1: Fetch/Submit all Site objects
            this.ScrapeSites();
            // Step 2: Fetch/Submit all Computer objects
            this.ScrapeComputers();
        }

        // Scrapes all sites through /api/sites
        private void ScrapeSites()
        {
            // Retrieve all Sites from the API
            List <Site> apiSites = API.GetSites();   
            // Retrieve all Sites in the DB
            IEnumerable<Site> dbSites = DB.Connection.Query<Site>(@"SELECT * FROM BESEXT.SITE");

            foreach (Site apiSite in apiSites)
            {         
                // Check if the Site already exists in the DB
                if (!dbSites.Any(s => s.Name == apiSite.Name))
                {
                    DB.Connection.Insert<Site>(apiSite);
                }
                else
                {
                    // Else update the Site
                    Site dbSite = dbSites.Where(s => s.Name == apiSite.Name).Single();
                    dbSite.Type = apiSite.Type;
                    
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
                // If the site in the db could not be retrieved using the API, delete it
                if (!apiSites.Any(s => s.Name == dbSite.Name))
                {
                    if (!DB.Connection.Delete(dbSite))
                    {
                        Exception e = new Exception("Unable to DELETE Site object");
                        e.Data["ObjectData"] = dbSite;
                        throw e;
                    }
                }
            }
        }

        // Scrapes all computers through /api/computers
        private void ScrapeComputers()
        {
            // Retrieve all Computers from the API
            List<Computer> apiComputers = API.GetComputers();
            // Retrieve all Computers in the DB
            IEnumerable<Computer> dbComputers = DB.Connection.Query<Computer>(@"SELECT * FROM BESEXT.COMPUTER");

            foreach (Computer apiComputer in apiComputers)
            {
                // Check if the Computer already exists in the DB
                if (!dbComputers.Any(c => c.ComputerID == apiComputer.ComputerID))
                {
                    DB.Connection.Insert<Computer>(apiComputer);
                }
                else
                {
                    // Else update the Site
                    Computer dbComputer = dbComputers.Where(c => c.ComputerID == apiComputer.ComputerID).Single();
                    dbComputer.LastReportTime = apiComputer.LastReportTime;

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
                // If the site in the db could not be retrieved using the API, delete it
                if (!apiComputers.Any(c => c.ComputerID == dbComputer.ComputerID))
                {
                    if (!DB.Connection.Delete(dbComputer))
                    {
                        Exception e = new Exception("Unable to DELETE Site object");
                        e.Data["ObjectData"] = dbComputer;
                        throw e;
                    }
                }
            }
        }
        
        private void ScrapeComputerGroups()
        {

        }
    }
}
