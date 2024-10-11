using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Shared.DTO
{
    public class FriendRequestDto
    {
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
    }

}
