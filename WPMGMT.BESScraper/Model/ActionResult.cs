using System;
using DapperExtensions.Mapper;

namespace WPMGMT.BESScraper.Model
{
    public class ActionResult
    {
        public ActionResult(int aActionID, int aComputerID, string aStatus, int aState, int aApplyCount, int aRetryCount, DateTime aStartTime, DateTime aEndTime)
        {
            this.ActionID = aActionID;
            this.ComputerID = aComputerID;        
            this.Status = aStatus;
            this.State = aState;
            this.ApplyCount = aApplyCount;
            this.RetryCount = aRetryCount;
            this.StartTime = aStartTime;
            this.EndTime = aEndTime;
        }

        public int ID { get; set; }
        public int ActionID { get; set; }
        public int ComputerID { get; set; }    
        public string Status { get; set; }
        public int State { get; set; }
        public int ApplyCount { get; set; }
        public int RetryCount { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

    // DapperExtensions Mapper for ActionResult Class
    public class ActionResultMapper : ClassMapper<ActionResult>
    {
        public ActionResultMapper()
        {
            Table("BESEXT.ACTION_RESULT");
            Map(f => f.ID).Column("ID").Key(KeyType.Identity);
            Map(f => f.ActionID).Column("ActionID");
            Map(f => f.ComputerID).Column("ComputerID");
            Map(f => f.Status).Column("Status");
            Map(f => f.State).Column("State");
            Map(f => f.ApplyCount).Column("ApplyCount");
            Map(f => f.RetryCount).Column("RetryCount");
            Map(f => f.StartTime).Column("StartTime");
            Map(f => f.EndTime).Column("EndTime");
        }
    }
}