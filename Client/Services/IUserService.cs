namespace Messenger.Client.Services
{
    using Messenger.Shared.DTO;
    using Messenger.Shared.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUserService
    {
        Task<List<Conv>> GetConv(string apiname);
        Task SendMessage(Conv message, string api);
        Task Seen(string api);
        Task<bool> Login(UserDto userDto, string api);
        Task<User> GetLoggedInUser();
        Task<List<User>> Friends(string apiname);
        Task<List<User>> NoFriends(string apiname);
        Task SendFriendReq(string apiname, FriendRequestDto friendRequest);
        Task<List<FriendUserDto>> GetAllUsers(string apiname);
        Task<bool> Accept(string apiname);
        Task<bool> Decline(string apiname);
    }
}