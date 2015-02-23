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

        public int ID           { get; set; }
        public int BaselineID   { get; set; }
        public int SiteID       { get; set; }
        public string Name      { get; set; }
    }
}
