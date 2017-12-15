using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace WebApiCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var appRootPath = Directory.GetCurrentDirectory();
            BuildWebHost(appRootPath, args).Run();
        }


        public static IWebHost BuildWebHost(string appRootPath, string[] args)
        {
            var webHostBuilder = GetWebHostBuilder(appRootPath, args);
            return webHostBuilder.Build();
        }


        public static IWebHostBuilder GetWebHostBuilder(string appRootPath, string[] args)
        {
            // use this to allow command line parameters in the config
            var configuration = new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build();

            var hostUrl = configuration["hosturl"];
            if (string.IsNullOrEmpty(hostUrl))
                hostUrl = "http://0.0.0.0:5000";

            var webHostBuilder = new WebHostBuilder()
                .UseConfiguration(configuration)
                //.UseUrls(hostUrl)
                .UseContentRoot(appRootPath)
                .UseKestrel()
                .UseIISIntegration()
                .UseStartup<Startup>();

            return webHostBuilder;
        }
    }
}
