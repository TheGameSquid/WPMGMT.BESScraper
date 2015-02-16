using System;
using DapperExtensions.Mapper;

namespace WPMGMT.BESScraper.Model
{
    public class Site
    {
        public Site()
        {
            // Constructor for Dapper and RestSharp
        }

        public Site(string aName, string aType)
        {
            this.Name = aName;
            this.Type = aType;
        }

        public int ID       { get; set; }   // Identity ID assigned by DB
        public string Name  { get; set; }
        public string Type  { get; set; }
    }

    // DapperExtensions Mapper for Site Class
    public class SiteMapper : ClassMapper<Site>
    {
        public SiteMapper()
        {
            // Define target Table and Schema
            Schema("BESEXT");
            Table("SITE");

            // Define target columns
            Map(f => f.ID).Column("ID").Key(KeyType.Identity);
            Map(f => f.Name).Column("Name");
            Map(f => f.Type).Column("Type");
        }
    }
}
