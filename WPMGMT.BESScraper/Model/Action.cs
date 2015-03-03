using System;
using DapperExtensions.Mapper;
using RestSharp.Deserializers;

namespace WPMGMT.BESScraper.Model
{
    public class Action
    {
        public Action()
        {
            // Empty constructor for Dapper and RestSharp
        }

        public Action(int actionID, int siteID, string name)
        {
            this.ActionID = actionID;
            this.SiteID = siteID;
            this.Name = name;
        }

        public int ID           { get; set; }       // Identity ID assigned by DB
        public int ActionID     { get; set; }       // Identity ID assigned by API
        public int SiteID       { get; set; }
        public string Name      { get; set; }
    }

    // DapperExtensions Mapper for Action Class
    public class ActionMapper : ClassMapper<Action>
    {
        public ActionMapper()
        {
            // Define target Table and Schema
            Schema("BESEXT");
            Table("ACTION");

            // Define target columns
            Map(f => f.ID).Column("ID").Key(KeyType.Identity);
            Map(f => f.ActionID).Column("ActionID");
            Map(f => f.SiteID).Column("SiteID");
            Map(f => f.Name).Column("Name");
        }
    }
}
