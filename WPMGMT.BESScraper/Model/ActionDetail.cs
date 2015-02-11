using System;
using System.Collections.Generic;

namespace WPMGMT.BESScraper.Model
{
    class ActionDetail
    {
        public ActionDetail(int aActionID, string aStatus, string aDateIssued)
        {
            this.ActionID = aActionID;
            this.Status = aStatus;
            this.DateIssued = aDateIssued;
            this.Computers = new List<ActionResult>();
        }

        public int ActionID { get; set; }
        public string Status { get; set; }
        public string DateIssued { get; set; }
        public List<ActionResult> Computers { get; set; }
    }
}
