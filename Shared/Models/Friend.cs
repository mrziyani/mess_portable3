using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Shared.Models
{
    public class Friend
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Sender")]
        public string IdEmet { get; set; }
        public User? Sender { get; set; } // Navigation property

        [ForeignKey("Receiver")]
        public string IdRec { get; set; }
        public User? Receiver { get; set; } // Navigation property

        [Required]
        public bool Etat { get; set; } // tinyint mapped to bool
    }
}


