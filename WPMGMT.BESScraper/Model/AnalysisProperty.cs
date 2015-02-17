using System;
using DapperExtensions.Mapper;

namespace WPMGMT.BESScraper.Model
{
    public class AnalysisProperty
    {
        public AnalysisProperty(int analysisID, string name)
        {
            this.AnalysisID = analysisID;
            this.Name = name;
        }

        public int ID           { get; set; }   // Identity ID assigned by DB
        public int AnalysisID   { get; set; }   // Parent Analysis API ID
        public string Name      { get; set; }
    }

    // DapperExtensions Mapper for AnalysisProperty Class
    public class AnalysisPropertyMapper : ClassMapper<AnalysisProperty>
    {
        public AnalysisPropertyMapper()
        {
            // Define target Table and Schema
            Schema("BESEXT");
            Table("ANALYSIS_PROPERTY");

            // Define target columns
            Map(f => f.ID).Column("ID").Key(KeyType.Identity);
            Map(f => f.AnalysisID).Column("AnalysisID");
            Map(f => f.Name).Column("Name");
        }
    }
}
