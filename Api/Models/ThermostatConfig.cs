using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MomitKiller.Api.Models
{
    public class ThermostatConfig
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id
        {
            get;
            set;
        }

        [Required]
        public decimal Setpoint
        {
            get;
            set;
        }

        [Required]
        public ThermostatModes Mode
        {
            get;
            set;
        }

        [Required]
        public decimal Hysteresis
        {
            get;
            set;
        }

        [Required]
        public decimal Calibration
        {
            get;
            set;
        }

        public ThermostatConfig()
        {
        }
    }
}
