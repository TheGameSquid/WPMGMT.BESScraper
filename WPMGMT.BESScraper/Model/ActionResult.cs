using System;
using RestSharp.Deserializers;

namespace WPMGMT.BESScraper.Model
{
    public class ActionResult
    {
        public ActionResult(int aComputerID, string aComputerName, string aStatus, int aState, int aApplyCount, int aRetryCount, string aStartTime, string aEndTime)
        {
            this.ComputerID = aComputerID;
            this.ComputerName = aComputerName;
            this.Status = aStatus;
            this.State = aState;
            this.ApplyCount = aApplyCount;
            this.RetryCount = aRetryCount;
            this.StartTime = aStartTime;
            this.EndTime = aEndTime;
        }

        public int ComputerID { get; set; }
        public string ComputerName { get; set; }
        public string Status { get; set; }
        public int State { get; set; }
        public int ApplyCount { get; set; }
        public int RetryCount { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}