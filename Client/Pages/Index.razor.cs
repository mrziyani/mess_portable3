using Messenger.Client.Services;
using Messenger.Shared.DTO;
using Microsoft.AspNetCore.Components;

namespace Messenger.Client.Pages
{
    public partial class Index
    {
        private string emailOrPhone;
        private string password;
        [Inject]
        public IUserService _user { get; set; }
        [Inject]
        NavigationManager _navigationManager { get; set; }

        private async Task Login()
        {
            UserDto userDto = new UserDto
            {
                Email = emailOrPhone,
                Mdp = password
            };

            bool isSuccess = await _user.Login(userDto, "api/Users/login");

            if (isSuccess)
            {
                Console.WriteLine("User logged in successfully.");
                _navigationManager.NavigateTo("ListeFriends");
            }
            else
            {
                Console.WriteLine("Login failed.");
            }
        }

    }
}
