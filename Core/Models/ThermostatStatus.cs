using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
namespace MomitKiller.Models
{
    [DataContract]
    public class ThermostatStatus
    {
        [DataMember(Name = "temperature")]
        public decimal Temperature
        {
            get;
            set;
        }

        [DataMember(Name = "setpoint")]
        public decimal Setpoint
        {
            get;
            set;
        }

        [DataMember(Name = "relayStatus")]
        public RelayStatuses RelayStatus
        {
            get;
            set;
        }

        [DataMember(Name = "mode")]
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
