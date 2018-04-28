using System;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Configuration;
using MomitKiller.Api.Models;
using System.Diagnostics;

namespace MomitKiller.Api.Services
{
    public class TemperatureService : ITemperatureService
    {
        private string _sensorPath;

        public TemperatureService(Settings settings)
        {
            _sensorPath = settings.GetValue<string>("SensorPath");
        }

        public async Task<decimal> GetCurrentAsync()
        {
            try
            {
                var sensorText = await File.ReadAllTextAsync(_sensorPath);
                var temperatureText = sensorText.Substring(sensorText.Length - 6);

                decimal temperature;

                if (!decimal.TryParse(temperatureText, out temperature))
                {
                    throw new Exception("Error parsing temperature");
                }

                return temperature / 1000;
            }
            catch(Exception ex)
            {
                throw new Exception("Error parsing temperature", ex);
            }
        }
    }
}
