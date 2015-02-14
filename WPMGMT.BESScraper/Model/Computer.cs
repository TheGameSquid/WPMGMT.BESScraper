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
        public string ComputerName          { get; set; }
        public string OS                    { get; set; }
        public string ActiveDirectoryPath   { get; set; }
        public bool Locked                  { get; set; }
        public DateTime LastReportTime      { get; set; }
    }

    // DapperExtensions Mapper for ActionDetail Class
    public class ComputerMapper : ClassMapper<Computer>
    {
        public ComputerMapper()
        {
            // Define target Table and Schema
            Schema("BESEXT");
            Table("ACTION");

            // Define target columns
            Map(f => f.ID).Column("ID").Key(KeyType.Identity);
            Map(f => f.ComputerID).Column("ComputerID").Key(KeyType.Assigned);
            Map(f => f.ComputerName).Column("ComputerName");
            Map(f => f.OS).Column("OS");
            Map(f => f.ActiveDirectoryPath).Column("ActiveDirectoryPath");
            Map(f => f.Locked).Column("Locked");
            Map(f => f.LastReportTime).Column("LastReportTime");
        }
    }
}
