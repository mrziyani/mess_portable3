using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Shared.Models
{
    public class Conv
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
        public string Message { get; set; }

        
        public int? Etat { get; set; }

        [Required]
        public DateTime Timestamp { get; set; } = DateTime.Now; // Default to current time
    }
}
