using System;
using DapperExtensions.Mapper;

namespace WPMGMT.BESScraper.Model
{
    public class AnalysisPropertyResult
    {
        public AnalysisPropertyResult()
        {
            // Empty constructor for Dapper and RestSharp
        }

        public AnalysisPropertyResult(int propertyID, int computerID, string value)
        {
            this.PropertyID = propertyID;
            this.ComputerID = computerID;
            this.Value = value;
        }

        public int ID           { get; set; }   // Identity ID assigned by DB
        public int PropertyID   { get; set; }   // Parent Property ID assigned by DB
        public int ComputerID   { get; set; }   // Parent Computer ID assigned by API
        public string Value     { get; set; }
    }

    // DapperExtensions Mapper for AnalysisPropertyResult Class
    public class AnalysisPropertyResultMapper : ClassMapper<AnalysisPropertyResult>
    {
        public AnalysisPropertyResultMapper()
        {
            // Define target Table and Schema
            Schema("BESEXT");
            Table("ANALYSIS_PROPERTY_RESULT");

            // Define target columns
            Map(f => f.ID).Column("ID").Key(KeyType.Identity);
            Map(f => f.PropertyID).Column("PropertyID");
            Map(f => f.ComputerID).Column("ComputerID");
            Map(f => f.Value).Column("Value");
        }
    }
}
