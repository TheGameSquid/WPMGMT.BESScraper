using System;
using DapperExtensions.Mapper;

namespace WPMGMT.BESScraper.Model
{
    public class BaselineResult
    {
        public BaselineResult()
        {
            // Empty constructor for Dapper and RestSharp
        }

        public BaselineResult(int baselineID, int computerID)
        {
            this.BaselineID = baselineID;
            this.ComputerID = computerID;
        }

        public int ID           { get; set; }
        public int BaselineID   { get; set; }
        public int ComputerID   { get; set; }

        // Overload for the default .Equals() method
        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to BaselineResult, return false.
            BaselineResult result = obj as BaselineResult;
            if ((System.Object)result == null)
            {
                return false;
            }

            // Return true if the fields match
            return (this.BaselineID == result.BaselineID) && (this.ComputerID == result.ComputerID); 
        }
    }

    // DapperExtensions Mapper for BaselineResult Class
    public class BaselineResultMapper : ClassMapper<BaselineResult>
    {
        public BaselineResultMapper()
        {
            // Define target Table and Schema
            Schema("BESEXT");
            Table("BASELINE_RESULT");

            // Define target columns
            Map(f => f.ID).Column("ID").Key(KeyType.Identity);
            Map(f => f.BaselineID).Column("BaselineID");
            Map(f => f.ComputerID).Column("ComputerID");
        }
    }
}
