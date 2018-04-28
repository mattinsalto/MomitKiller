using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MomitKiller.Api.Models
{
    public class Temperature
    {
        [Key]
        public int Id
        {
            get;
            set;
        }

        [Required]
        public DateTime Date
        {
            get;
            set;
        }

        [Required]
        public decimal Value
        {
            get;
            set;
        }

        public Temperature()
        {
            Date = DateTime.Now;
        }
    }
}
