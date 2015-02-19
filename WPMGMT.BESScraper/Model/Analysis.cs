using System;
using DapperExtensions.Mapper;
using RestSharp.Deserializers;

namespace WPMGMT.BESScraper.Model
{
    public class Analysis
    {
        public Analysis()
        {
            // Empty constructor for Dapper and RestSharp
        }

        [DeserializeAs(Name = "IgnoreID")]
        public int ID           { get; set; }       // Identity ID assigned by DB
        [DeserializeAs(Name = "ID")]
        public int AnalysisID   { get; set; }       // Identity ID assigned by API
        public int SiteID       { get; set; }
        public string Name      { get; set; }
    }

    // DapperExtensions Mapper for Analysis Class
    public class AnalysisMapper : ClassMapper<Analysis>
    {
        public AnalysisMapper()
        {
            // Define target Table and Schema
            Schema("BESEXT");
            Table("ANALYSIS");

            // Define target columns
            Map(f => f.ID).Column("ID").Key(KeyType.Identity);
            Map(f => f.AnalysisID).Column("AnalysisID");
            Map(f => f.SiteID).Column("SiteID");
            Map(f => f.Name).Column("Name");
        }
    }
}
