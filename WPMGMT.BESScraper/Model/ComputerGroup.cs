using System;
using DapperExtensions.Mapper;
using RestSharp.Deserializers;

namespace WPMGMT.BESScraper.Model
{
    public class ComputerGroup
    {
        public ComputerGroup()
        {
            // Empty constructor for Dapper and RestSharp
        }

        [DeserializeAs(Name = "IgnoreID")]
        public int ID           { get; set; }   // Identity ID assigned by DB
        [DeserializeAs(Name = "ID")]
        public int GroupID      { get; set; }   // Identity ID assigned by API
        public int SiteID       { get; set; }
        public string Name      { get; set; }
        public bool Manual      { get; set; }
    }

    // DapperExtensions Mapper for ComputerGroup Class
    public class ComputerGroupMapper : ClassMapper<ComputerGroup>
    {
        public ComputerGroupMapper()
        {
            // Define target Table and Schema
            Schema("BESEXT");
            Table("GROUP");

            // Define target columns
            Map(f => f.ID).Column("ID").Key(KeyType.Identity);
            Map(f => f.GroupID).Column("GroupID");
            Map(f => f.SiteID).Column("SiteID");
            Map(f => f.Name).Column("Name");
            Map(f => f.Manual).Column("Manual");
        }
    }
}
