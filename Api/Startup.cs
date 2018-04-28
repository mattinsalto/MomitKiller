using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MomitKiller.Api.Services;
using MomitKiller.Api.Models;
using MomitKiller.Api.Middleware;
using Microsoft.EntityFrameworkCore;

namespace MomitKiller.Api
{
    public class Startup
    {
        private IHostingEnvironment _env;
        private IConfiguration _appSettings;
        private ThermostatConfig _thermostatConfig;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            _appSettings = configuration;
            _env = env;
            Init();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services
                .AddDbContext<MomitKillerDbContext>(options => options
                                                    .UseSqlite(_appSettings
                                                               .GetConnectionString("MomitKiller")));

            services.AddSingleton<ThermostatConfig>(_thermostatConfig);
            services.AddSingleton<Settings>(new Settings(_appSettings));

            if (_env.IsDevelopment())
            {
                services.AddTransient<ITemperatureService, TemperatureServiceFake>();
                services.AddTransient<IRelayService, RelayServiceFake>();
                //services.AddTransient<IRelayService, RelayService>();
            }
            else
            {
                services.AddTransient<ITemperatureService, TemperatureService>();
                services.AddTransient<IRelayService, RelayService>();
            }

            services.AddTransient<IThermostatService, ThermostatService>();
            services.Configure<EmailSettings>(_appSettings.GetSection("EmailSettings"));
            services.AddTransient<IMailService, MailService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseApiKeyAuthentication();
            app.UseMvc();
        }

        /// <summary>
        /// Initializes thermostat default values from appsettings.json 
        /// if they don't exist in DB
        /// </summary>
        public void Init()
        {
            var connectionString = _appSettings.GetConnectionString("MomitKiller");

            using (var db = new MomitKillerDbContext(connectionString))
            {
                _thermostatConfig = db.ThermostatConfig.FirstOrDefault();
                if (_thermostatConfig == null)
                {
                    var thermostatDefaults = _appSettings
                                            .GetSection("ThermostatDefaults");

                    _thermostatConfig = new ThermostatConfig()
                    {
                        Id = 1,
                        Setpoint = thermostatDefaults
                                  .GetValue<decimal>("Temperature"),

                        Hysteresis = thermostatDefaults
                                    .GetValue<decimal>("Hysteresis"),

                        Calibration = thermostatDefaults
                                    .GetValue<decimal>("Calibration"),

                        Mode = thermostatDefaults
                                    .GetValue<ThermostatModes>("Mode")
                    };

                    db.ThermostatConfig.Add(_thermostatConfig);
                    db.SaveChanges();
                }
            }
        }
    }
}
