using System;
using DapperExtensions.Mapper;

namespace WPMGMT.BESScraper.Model
{
    public class Action
    {
        public Action()
        {
            // Empty constructor for use by RestSharp
        }

        public int ID           { get; set; }       // Identity ID assigned by DB
        public int ActionID     { get; set; }       // Identity ID assigned by API
        public string Name      { get; set; }
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
