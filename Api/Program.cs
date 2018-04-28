using System;
using System.Net;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using MomitKiller.Api.Services;
using MomitKiller.Api.Models;
using System.Threading;

namespace MomitKiller.Api
{
    public class Program
    {
        private static IWebHost _webhost;
        private static Timer _timer;

        public static void Main(string[] args)
        {
            _webhost = BuildWebHost(args);

            _timer = new Timer(CheckTemperature, null, 1000, 25000);

            _webhost.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .UseStartup<Startup>()
                   .UseKestrel(options =>
                    {
                        options.Listen(IPAddress.Any, 5000);
                    })
                    .Build();

        private static void CheckTemperature(object stateInfo)
        {
            try
            {
                using (var scope = _webhost.Services.GetService<IServiceScopeFactory>().CreateScope())
                {
                    var thermostatService = scope.ServiceProvider.GetService<IThermostatService>();
                    thermostatService.CheckAsync();
                }
            }
            catch (Exception ex)
            {
                using (var scope = _webhost.Services.GetService<IServiceScopeFactory>().CreateScope())
                {
                    var mailService = scope.ServiceProvider.GetService<IMailService>();
                    mailService.SendEmailAsync("Error in Program CheckTemperature", ex.ToString());
                }

                Console.WriteLine(ex.ToString());
            }
        }
    }
}
