using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;

using RestSharp;
using Dapper;
using DapperExtensions;

using WPMGMT.BESScraper.Model;
using WPMGMT.BESScraper.API;

namespace WPMGMT.BESScraper
{
    class Program
    {
        // TODO: DateTimeFormat
        static void Main(string[] args)
        {
            BesApi besApi = new BesApi(
                                    ConfigurationManager.AppSettings["ApiEndpoint"],
                                    ConfigurationManager.AppSettings["ApiUser"],
                                    ConfigurationManager.AppSettings["ApiPassword"]);
            BesDb besDb = new BesDb(ConfigurationManager.ConnectionStrings["DB"].ToString());

            Scraper scraper = new Scraper(besApi, besDb);
            scraper.Run();

            Console.WriteLine("All done :)");
            Console.Read();
        }
    }
}