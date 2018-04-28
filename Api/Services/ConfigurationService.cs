using System;
using System.Threading.Tasks;
using System.Linq;
using MomitKiller.Api.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace MomitKiller.Api.Services
{
    public class ConfigurationService
    {
        private Configuration _configuration;
        private IConfiguration _appConfig;

        public float Temperature
        {
            get
            {
                return _configuration.Temperature;
            }
        }

        public float Hysteresis
        {
            get
            {
                return _configuration.Hysteresis;
            }
        }

        public ConfigurationService(IConfiguration appConfig)
        {
            _appConfig = appConfig;
            Init();
        }

        /// <summary>
        /// Initializes thermostat default values from appsettings.json 
        /// if they don't exist in DB
        /// </summary>
        public void Init()
        {
            var connectionString = _appConfig.GetConnectionString("MomitKiller");

            using (var db = new MomitKillerDbContext(connectionString))
            {
                _configuration = db.Configuration.FirstOrDefault();
                if (_configuration == null)
                {
                    var thermostatDefaults = _appConfig
                                            .GetSection("ThermostatDefaults");

                    _configuration = new Configuration()
                    {
                        Id = 1,
                        Temperature = thermostatDefaults
                                      .GetValue<float>("Temperature"),
                        
                        Hysteresis = thermostatDefaults
                                    .GetValue<float>("Hysteresis")
                    };

                    db.Configuration.Add(_configuration);
                    db.SaveChanges();
                }
            }
        }

        public void SetTemperature(float temperature)
        {
            
        }
    }
}
