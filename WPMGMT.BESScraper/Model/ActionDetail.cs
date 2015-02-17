using System;
using System.Collections.Generic;
using DapperExtensions.Mapper;

namespace WPMGMT.BESScraper.Model
{
    public class ActionDetail
    {
        public ActionDetail(int aActionID, string aStatus, string aDateIssued)
        {
            this.ActionID = aActionID;
            this.Status = aStatus;
            this.DateIssued = aDateIssued;
        }

        public int ID               { get; set; }       // Identity ID assigned by DB
        public int ActionID         { get; set; }
        public string Status        { get; set; }
        public string DateIssued    { get; set; }
    }

    // DapperExtensions Mapper for ActionDetail Class
    public class ActionDetailMapper : ClassMapper<ActionDetail>
    {
        public ActionDetailMapper()
        {
            // Define target Table and Schema
            Schema("BESEXT");
            Table("ACTION_DETAIL");

            // Define target columns
            Map(f => f.ID).Column("ID").Key(KeyType.Identity);
            Map(f => f.ActionID).Column("ActionID");
            Map(f => f.Status).Column("Status");
            Map(f => f.DateIssued).Column("DateIssued");
        }
    }
}
