using System;
using System.Collections.Generic;
using System.Net;
using RestSharp;

using WPMGMT.BESScraper.Model;

namespace WPMGMT.BESScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            Scraper scraper = new Scraper("https://pc120006933:52311/api/", "iemadmin", "bigfix");
            List<WPMGMT.BESScraper.Model.Action> actions = scraper.GetActions();

            ActionDetail dt = scraper.GetActionDetail(1860);

            Console.Read();
        }
    }
}
