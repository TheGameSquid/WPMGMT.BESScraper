using System;
using System.Collections.Generic;

using PetaPoco;

namespace WPMGMT.BESScraper.Model
{
    [TableName("BESEXT.ACTION")]
    [PrimaryKey("ID_DB", autoIncrement = true)]
    public class Action : Model
    {
        public Action()
        {
            
        }



        public int DB_ID { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class Actions
    {
        public Actions()
        {
            // Empty constructor for 
        }

        public List<Action> ActionList { get; set; }
    }
}
