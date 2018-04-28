using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MomitKiller.Api.Models
{
    public class Configuration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id
        {
            get;
            set;
        }

        [Required]
        public float Temperature
        {
            get;
            set;
        }

        public float Hysteresis
        {
            get;
            set;
        }
    }
}
