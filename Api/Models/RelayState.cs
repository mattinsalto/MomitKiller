using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MomitKiller.Api.Models
{
    public class RelayState
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public RelayState()
        {
            
        }
    }
}
