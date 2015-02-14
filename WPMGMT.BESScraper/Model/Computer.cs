using System;
using DapperExtensions.Mapper;

namespace WPMGMT.BESScraper.Model
{
    public class Computer
    {
        public Computer()
        {
            // Empty constructor used by RestSharp
        }

        public int ID                       { get; set; }       // Identity ID assigned by DB
        public int ComputerID               { get; set; }       // Identity ID assigned by API
        public DateTime LastReportTime      { get; set; }
    }

    // DapperExtensions Mapper for ActionDetail Class
    public class ComputerMapper : ClassMapper<Computer>
    {
        public ComputerMapper()
        {
            // Define target Table and Schema
            Schema("BESEXT");
            Table("COMPUTER");

            // Define target columns
            Map(f => f.ID).Column("ID").Key(KeyType.Identity);
            Map(f => f.ComputerID).Column("ComputerID").Key(KeyType.Assigned);
            Map(f => f.LastReportTime).Column("LastReportTime");
        }
    }
}
