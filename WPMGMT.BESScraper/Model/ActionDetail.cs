using System;
using System.Collections.Generic;
using DapperExtensions.Mapper;

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

        public int ID { get; set; }
        public int ActionID { get; set; }
        public string Status { get; set; }
        public string DateIssued { get; set; }
        public List<ActionResult> Computers { get; set; }
    }

    // DapperExtensions Mapper for ActionDetail Class
    public class ActionDetailMapper : ClassMapper<ActionDetail>
    {
        public ActionDetailMapper()
        {
            Table("BESEXT.ACTION_DETAIL");
            Map(f => f.ID).Column("ID").Key(KeyType.Identity);
            Map(f => f.ActionID).Column("ActionID").Key(KeyType.Assigned);
            Map(f => f.Status).Column("Status");
            Map(f => f.DateIssued).Column("DateIssued");
        }
    }
}
