using System;
using System.Threading.Tasks;
using MomitKiller.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace MomitKiller.Api.Services
{
    public class ThermostatService : IThermostatService
    {
        private ThermostatConfig _thermostatConfig;
        private ITemperatureService _temperatureService;
        private IRelayService _relayService;
        private MomitKillerDbContext _db;

        public ThermostatService(ThermostatConfig thermostatConfig,
                                ITemperatureService temperatureService,
                                IRelayService relayService,
                                MomitKillerDbContext db)
        {
            _thermostatConfig = thermostatConfig;
            _temperatureService = temperatureService;
            _relayService = relayService;
            _db = db;
        }

        public async Task CheckAsync()
        {
            if (_thermostatConfig.Mode == ThermostatModes.Off)
                return;
            
            var currentTemperature = await _temperatureService.GetCurrentAsync();
            var setpoint = _thermostatConfig.Setpoint;
            var hysteresis = _thermostatConfig.Hysteresis;

            if (currentTemperature - hysteresis >= setpoint)
            {
                await _relayService.PowerOffAsync();
            }
            else if(await _relayService.GetStatusAsync() == RelayStatuses.Off)
            {
                await _relayService.PowerOnAsync();
            }

            PrintResult(currentTemperature,
                       setpoint,
                        hysteresis);
        }

        public async Task<ThermostatStatus> GetStatusAsync()
        {
            return new ThermostatStatus()
            {
                Temperature = await _temperatureService.GetCurrentAsync(),
                Setpoint = _thermostatConfig.Setpoint,
                RelayStatus = await _relayService.GetStatusAsync(),
                Mode = _thermostatConfig.Mode
            };
        }

        public async Task<ThermostatStatus> SetSetpointAsync(decimal setpoint)
        {
            _thermostatConfig.Setpoint = setpoint;
            _db.Entry(_thermostatConfig).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            await CheckAsync();

            return await GetStatusAsync();
        }

        public async Task<ThermostatStatus> SetModeAsync(ThermostatModes mode)
        {
            _thermostatConfig.Mode = mode;
            _db.Entry(_thermostatConfig).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            if(mode == ThermostatModes.Off)
            {
                await _relayService.PowerOffAsync();
            }
            else
            {
                await CheckAsync();
            }

            return await GetStatusAsync();
        }

        public async Task<decimal> GetHysteresisAsync()
        {
            return await Task.FromResult<decimal>(_thermostatConfig.Hysteresis);
        }

        public async Task SetHysteresisAsync(decimal hysteresis)
        {
            _thermostatConfig.Hysteresis = hysteresis;
            _db.Entry(_thermostatConfig).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task<decimal> GetCalibrationAsync()
        {
            return await Task.FromResult<decimal>(_thermostatConfig.Calibration);
        }

        public async Task SetCalibrationAsync(decimal calibration)
        {
            _thermostatConfig.Calibration = calibration;
            _db.Entry(_thermostatConfig).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        private void PrintResult(decimal currentTemperature,
                                decimal setpoint,
                                decimal hysteresis)
        {
            Console.WriteLine($"currentTemperature: {currentTemperature.ToString("00.0")}");
            Console.WriteLine($"setpoint: {setpoint.ToString("00.0")}");
            Console.WriteLine($"hysteresis: {hysteresis.ToString("00.0")}");
            Console.WriteLine($"state: {currentTemperature - hysteresis < setpoint}");
        }
    }
}
