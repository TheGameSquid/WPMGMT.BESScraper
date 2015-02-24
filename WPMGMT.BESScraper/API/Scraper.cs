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
                // Check if the site already exists in the DB
                Site dbSite = dbSites.Where(s => s.Name == apiSite.Name).Single();
                
                if (dbSite == null)
                {
                    // If the retrieved value is NULL, then the Site didn't exist yet
                    DB.Connection.Insert<Site>(apiSite);
                }
                else
                {
                    // Else update the Site
                    dbSite.Type = apiSite.Type;
                    // Throw exception if the update failed
                    if (!DB.Connection.Update<Site>(dbSite))
                    {
                        Exception e = new Exception("Unable to UPDATE Site object");
                        e.Data["UpdateData"] = dbSite;
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
                // Check if the site already exists in the DB
                Computer dbComputer = dbComputers.Where(c => c.ComputerID == apiComputer.ComputerID).Single();

                if (dbComputer == null)
                {
                    // If the retrieved value is NULL, then the Site didn't exist yet
                    DB.Connection.Insert<Computer>(apiComputer);
                }
                else
                {
                    // Else update the Site
                    dbComputer.LastReportTime = apiComputer.LastReportTime;
                    if (!DB.Connection.Update<Computer>(dbComputer))
                    {
                        Exception e = new Exception("Unable to UPDATE Computer object");
                        e.Data["UpdateData"] = dbComputer;
                        throw e;
                    }
                }
            }
        }

        private void ScrapeComputers()
        {

        }
    }
}
