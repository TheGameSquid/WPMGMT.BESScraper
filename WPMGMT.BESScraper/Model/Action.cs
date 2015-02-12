using System;
using System.Collections.Generic;
using System.Linq;
using DapperExtensions.Mapper;

namespace WPMGMT.BESScraper.Model
{
    public class Action
    {
        public int ID { get; set; }
        public int ActionID { get; set; }
        public string Name { get; set; }
    }

    // DapperExtensions Mapper for Action Class
    public class ActionMapper : ClassMapper<Action>
    {
        public ActionMapper()
        {
            // Define target Table and Schema
            Schema("BESEXT");
            Table("ACTION");

            // Define target columns
            Map(f => f.ID).Column("ID").Key(KeyType.Identity);
            Map(f => f.ActionID).Column("ActionID").Key(KeyType.Assigned);
            Map(f => f.Name).Column("Name");
        }
    }
}
