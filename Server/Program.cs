using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Util;

namespace Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            LoggingUtil.ConfigureNLog();

            var builder = CreateHostBuilder(args);
            builder.Build().RunAsync();


            Console.WriteLine("Holding main thread.");
            while (true)
            {
                Thread.Sleep(1000);
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
