using System;
using DapperExtensions.Mapper;
using RestSharp.Deserializers;

namespace WPMGMT.BESScraper.Model
{
    public class ComputerGroup
    {
        public ComputerGroup()
        {
            // Empty constructor used by RestSharp
        }

        public int ID           { get; set; }   // Identity ID assigned by DB
        public int GroupID      { get; set; }   // Identity ID assigned by API
        [DeserializeAs(Name = "Title")]
        public string Name      { get; set; }
        public string Domain    { get; set; }
    }

    // DapperExtensions Mapper for ActionDetail Class
    public class ComputerGroupMapper : ClassMapper<ComputerGroup>
    {
        public ComputerGroupMapper()
        {
            // Define target Table and Schema
            Schema("BESEXT");
            Table("GROUP");

            // Define target columns
            Map(f => f.ID).Column("ID").Key(KeyType.Identity);
            Map(f => f.GroupID).Column("GroupID").Key(KeyType.Assigned);
            Map(f => f.Name).Column("Name");
            Map(f => f.Domain).Column("Domain");
        }
    }
}
