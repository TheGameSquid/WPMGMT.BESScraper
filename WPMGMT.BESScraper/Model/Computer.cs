using System;
using DapperExtensions.Mapper;
using RestSharp.Deserializers;

namespace WPMGMT.BESScraper.Model
{
    public class Computer
    {
        public Computer()
        {
            // Empty constructor used by RestSharp
        }

        [DeserializeAs(Name = "IgnoreID")]
        public int ID                       { get; set; }       // Identity ID assigned by DB
        [DeserializeAs(Name = "ID")]
        public int ComputerID               { get; set; }       // Identity ID assigned by API
        public string ComputerName          { get; set; }
        public DateTime LastReportTime      { get; set; }
    }

    // DapperExtensions Mapper for Computer Class
    public class ComputerMapper : ClassMapper<Computer>
    {
        public ComputerMapper()
        {
            // Define target Table and Schema
            Schema("BESEXT");
            Table("COMPUTER");

            // Define target columns
            Map(f => f.ID).Column("ID").Key(KeyType.Identity);
            Map(f => f.ComputerID).Column("ComputerID");
            Map(f => f.ComputerName).Column("ComputerName");
            Map(f => f.LastReportTime).Column("LastReportTime");
        }
    }
}
