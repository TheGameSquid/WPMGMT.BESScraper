using System;
using RestSharp.Deserializers;

namespace WPMGMT.BESScraper.Model
{
    public class ComputerGroup
    {
        public ComputerGroup()
        {
            // Empty constructor used by RestSharp
        }

        public int ID           { get; set; }   // Identity ID assigned by DB
        public int GroupID      { get; set; }   // Identity ID assigned by API
        [DeserializeAs(Name = "Title")]
        public string Name      { get; set; }
        public string Domain    { get; set; }
    }
}
