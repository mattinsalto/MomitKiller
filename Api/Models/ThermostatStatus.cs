using System;
using MomitKiller.Api.Services;

namespace MomitKiller.Api.Models
{
    public class ThermostatStatus
    {
        public decimal Temperature
        {
            get;
            set;
        }
        public decimal Setpoint
        {
            get;
            set;
        }
        public RelayStatuses RelayStatus
        {
            get;
            set;
        }
        public ThermostatModes Mode
        {
            get;
            set;
        }

        public ThermostatStatus()
        {
            
        }
    }
}
