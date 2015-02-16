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
            return this.Connection.Query<Site>("SELECT * FROM BESEXT.SITE WHERE Name = @Name", new { Name = name }).Single();
        }

        public void InsertSite(Site site)
        {
            if (SelectSite(site.Name) != null)
            {
                Connection.Open();
                Site newSite = Connection.Insert<Site>(site);
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