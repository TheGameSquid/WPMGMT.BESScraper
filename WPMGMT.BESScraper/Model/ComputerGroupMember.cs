using System;
using DapperExtensions.Mapper;

namespace WPMGMT.BESScraper.Model
{
    public class ComputerGroupMember
    {
        public ComputerGroupMember(int GroupID, int ComputerID)
        {
            this.GroupID = GroupID;
            this.ComputerID = ComputerID;
        }

        public int ID           { get; set; }   // Identity ID assigned by DB
        public int GroupID      { get; set; }
        public int ComputerID   { get; set; }
    }

    // DapperExtensions Mapper for ActionDetail Class
    public class ComputerGroupMemberMapper : ClassMapper<ComputerGroupMember>
    {
        public ComputerGroupMemberMapper()
        {
            // Define target Table and Schema
            Schema("BESEXT");
            Table("GROUP_MEMBER");

            // Define target columns
            Map(f => f.ID).Column("ID").Key(KeyType.Identity);
            Map(f => f.GroupID).Column("GroupID");
            Map(f => f.ComputerID).Column("ComputerID");
        }
    }
}
