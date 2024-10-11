using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Shared.Models
{
    public class User
    {
        [Key]
        public string Id { get; set; } // Changed to string

        [Required]
        [MaxLength(50)]
        public string Prenom { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nom { get; set; }

        [Required]
        public DateTime DateNais { get; set; }

        [Required]
        public string Sexe { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MaxLength(255)]
        public string Mdp { get; set; }

        public ICollection<Conv>? SentConversations { get; set; } // Sent messages
        public ICollection<Conv>? ReceivedConversations { get; set; } // Received messages

        public ICollection<Friend>? Friends { get; set; }
    }
}


