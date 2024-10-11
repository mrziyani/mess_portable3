using Messenger.Shared.DTO;
using Messenger.Shared.Models;

namespace Messenger.Server.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task SendFriendReq(string senderId = "1", string receiverId = "2");
        Task AcceptFriendReq(int id);
        Task DeclinedFriendReq(int id);

        Task SendMsg(Conv conv);
        Task Seen(string idemet, string idrec);
        Task<List<User>> Friends(string id);
        Task<List<User>> NoFriends(string id);

        Task<List<Conv>> Conversation(string idemet, string idrec);

        Task<User> Login(UserDto userDto);

        Task<IEnumerable<FriendUserDto>> GetAllUsers(string userId);
    }  
}
