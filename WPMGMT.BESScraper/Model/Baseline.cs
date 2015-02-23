using System;
using DapperExtensions.Mapper;

namespace WPMGMT.BESScraper.Model
{
    public class Baseline
    {
        public Baseline()
        {
            // Empty constructor for Dapper and RestSharp
        }

        public Baseline(int baselineID, int siteID, string name)
        {
            this.BaselineID = baselineID;
            this.SiteID = siteID;
            this.Name = name;
        }

        public int ID           { get; set; }
        public int BaselineID   { get; set; }
        public int SiteID       { get; set; }
        public string Name      { get; set; }
    }

    // DapperExtensions Mapper for Baseline Class
    public class BaselineMapper : ClassMapper<Baseline>
    {
        public BaselineMapper()
        {
            // Define target Table and Schema
            Schema("BESEXT");
            Table("BASELINE");

            // Define target columns
            Map(f => f.ID).Column("ID").Key(KeyType.Identity);
            Map(f => f.BaselineID).Column("BaselineID");
            Map(f => f.SiteID).Column("SiteID");
            Map(f => f.Name).Column("Name");
        }
    }
}
