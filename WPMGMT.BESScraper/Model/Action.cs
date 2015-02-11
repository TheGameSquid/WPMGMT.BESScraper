using System;
using System.Collections.Generic;

namespace WPMGMT.BESScraper.Model
{
    public class Action : Model
    {
        public Action()
        {
            
        }

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
