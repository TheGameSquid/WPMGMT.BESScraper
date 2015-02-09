using System;
using RestSharp.Deserializers;

namespace WPMGMT.BESScraper.Model
{
    class Computer
    {
        public Computer()
        {
            // Empty constructor for 
        }

        [DeserializeAs(Name = "ID")]
        public int ComputerID { get; set; }
        [DeserializeAs(Name = "Name")]
        public string ComputerName { get; set; }
        public string Status { get; set; }
        public int State { get; set; }
        public int ApplyCount { get; set; }
        public int RetryCount { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}