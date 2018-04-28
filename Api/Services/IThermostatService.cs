using System;
using System.Threading.Tasks;
using MomitKiller.Api.Models;

namespace MomitKiller.Api.Services
{
    public interface IThermostatService
    {
        Task CheckAsync();
        Task<ThermostatStatus> GetStatusAsync();
        Task<ThermostatStatus> SetSetpointAsync(decimal setpoint);
        Task<ThermostatStatus> SetModeAsync(ThermostatModes mode);
        Task<decimal> GetHysteresisAsync();
        Task SetHysteresisAsync(decimal hysteresis);
        Task<decimal> GetCalibrationAsync();
        Task SetCalibrationAsync(decimal calibration);
    }
}
