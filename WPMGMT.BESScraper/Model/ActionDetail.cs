using System;
using System.Collections.Generic;

namespace WPMGMT.BESScraper.Model
{
    class ActionDetail
    {
        public ActionDetail()
        {
            // Empty constructor for 
        }

        public int ActionID { get; set; }
        public string Status { get; set; }
        public DateTime DateIssued { get; set; }
        public List<Computer> Computers { get; set; }
    }
}
