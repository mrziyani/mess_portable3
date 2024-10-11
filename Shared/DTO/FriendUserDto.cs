using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Shared.DTO
{
    public class FriendUserDto
    {
       
        public string Id { get; set; }
        public string IdUser { get; set; }
        public string Idemet { get; set; }
        public string UserName { get; set; }
        public string UserPrenom { get; set; }
        
        public bool? etat { get; set; }
       
    }

}
