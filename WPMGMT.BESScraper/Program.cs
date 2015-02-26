using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;

using Common.Logging;
using RestSharp;
using Dapper;
using DapperExtensions;
using Quartz;
using Topshelf;
using Topshelf.Quartz;

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
                                    ConfigurationManager.AppSettings["ApiPassword"]
                                );
            BesDb besDb = new BesDb(ConfigurationManager.ConnectionStrings["DB"].ToString());


            //Setup logging
            LogManager.Adapter = new Common.Logging.Simple.TraceLoggerFactoryAdapter { Level = LogLevel.Info };

            HostFactory.Run(x =>
            {
                x.RunAs(@"MSNET\EXF284", "frederik05");

                x.SetDescription("Data scraper for BigFix Enterprise Server");
                x.SetDisplayName("WPMGMT.BESScraper");
                x.SetServiceName("WPMGMT.BESScraper");

                x.ScheduleQuartzJobAsService(q =>
                    q.WithJob(() =>
                        JobBuilder.Create<Scraper>().Build())
                            .AddTrigger(() =>
                                TriggerBuilder.Create()
                                    .WithCronSchedule(ConfigurationManager.AppSettings["CronExpression"])
                                    .StartNow()
                                    .Build())
                );
            });
        }
    }
}