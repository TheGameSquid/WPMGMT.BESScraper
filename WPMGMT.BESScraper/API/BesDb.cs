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

        public WPMGMT.BESScraper.Model.Action SelectAction(int id)
        {
            IEnumerable<WPMGMT.BESScraper.Model.Action> actions = this.Connection.Query<WPMGMT.BESScraper.Model.Action>("SELECT * FROM BESEXT.ACTION WHERE ID = @Id", new { Id = id });
            if (actions.Count() > 0)
            {
                return actions.Single();
            }
            return null;
        }

        public WPMGMT.BESScraper.Model.Action SelectAction(string name)
        {
            IEnumerable<WPMGMT.BESScraper.Model.Action> actions = this.Connection.Query<WPMGMT.BESScraper.Model.Action>("SELECT * FROM BESEXT.ACTION WHERE Name = @Name", new { Name = name });
            if (actions.Count() > 0)
            {
                return actions.Single();
            }
            return null;
        }

        public ActionDetail SelectActionDetail(int id)
        {
            IEnumerable<ActionDetail> details = this.Connection.Query<ActionDetail>("SELECT * FROM BESEXT.ACTION_DETAIL WHERE ActionID = @Id", new { Id = id });
            if (details.Count() > 0)
            {
                return details.Single();
            }
            return null;
        }

        public Computer SelectComputer(string hostName)
        {

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
            if (SelectAction(action.Name) == null)
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