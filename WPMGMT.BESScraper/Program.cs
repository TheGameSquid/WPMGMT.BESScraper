using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;

using RestSharp;

using WPMGMT.BESScraper.Model;

namespace WPMGMT.BESScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            BesApi besApi = new BesApi(new Uri("https://pc120006933:52311/api/"), "iemadmin", "bigfix");
            //WPMGMT.BESScraper.Model.Action action = besApi.GetAction(1854);
            //Actions actions = besApi.GetActions();
            //ActionDetail detail = besApi.GetActionDetail(1854);

            WPMGMT.BESScraper.Model.Action action = new Model.Action();
            action.ID = 1855;
            action.Name = "TEST POC TROLL";

            Console.Read();
        }
    }
}
