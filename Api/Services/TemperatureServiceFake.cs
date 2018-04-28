using System;
using System.Threading.Tasks;
using MomitKiller.Api.Models;

namespace MomitKiller.Api.Services
{
    public class TemperatureServiceFake : ITemperatureService
    {
        public TemperatureServiceFake()
        {
        }

        public async Task<decimal> GetCurrentAsync()
        {
            var random = new Random();
            var temperture = (decimal)(random.Next(-10, 40) + random.NextDouble());

            return await Task.FromResult<decimal>(temperture);
        }
    }
}
