using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;

using RestSharp;
using PetaPoco;

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

            Database db = new PetaPoco.Database("TEST");

            bool LOL = db.Exists<Model.Action>("ID = '1855'");
            

            //int modelId = multiKey.Name;

            //using (SqlConnection cn = new SqlConnection(@"Data Source=10.50.20.128\YPTOSQL002LP;Initial Catalog=YPTO_WPMGMT;Integrated Security=SSPI;"))
            //{
            //    //cn.Open();
            //    //var multiKey = cn.Insert(action);
            //    //cn.Close();

            //    //int modelId = multiKey.Name;
            //}

            Console.Read();
        }
    }
}
