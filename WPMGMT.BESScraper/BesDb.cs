using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using DapperExtensions;
using WPMGMT.BESScraper.Model;

namespace WPMGMT.BESScraper
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

        public Site SelectSite(string name)
        {
            IEnumerable<Site> sites = this.Connection.Query<Site>("SELECT * FROM BESEXT.SITE WHERE Name = @Name", new { Name = name });
            if (sites.Count() > 0)
            {
                return sites.Single();
            }
            return null;       
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