using System;
using System.Collections.Generic;
using System.Linq;
using DapperExtensions.Mapper;

namespace WPMGMT.BESScraper.Model
{
    public class Action : Model
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
            Table("BESEXT.ACTION");
            Map(f => f.ID).Column("ID").Key(KeyType.Identity);
            Map(f => f.ActionID).Column("ActionID").Key(KeyType.Assigned);
            Map(f => f.Name).Column("Name");
        }
    }
}
