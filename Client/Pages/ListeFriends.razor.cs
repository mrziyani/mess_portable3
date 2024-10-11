
using Messenger.Shared.Models;
using Microsoft.AspNetCore.Components;
using Messenger.Client.Services;

namespace Messenger.Client.Pages
{
    public partial class ListeFriends
    {

        [Inject]
        public IMainService<User> _user{ get; set; }

        [Inject]
        public IUserService _user2 { get; set; }

        // Change List<Products> to List<Product>
        List<User> users { get; set; } = new List<User>();

        protected override async Task OnInitializedAsync()
        {
            var loggedInUser = await _user2.GetLoggedInUser();
            // Change Products to Product in the GetFromJsonAsync call
            users = await _user2.Friends($"/api/Users/friends/{loggedInUser.Id}");
           

            // Check if the logged-in user is not null
            if (loggedInUser != null)
            {
                Console.WriteLine($"Logged in as: {loggedInUser.Email} with the id : {loggedInUser.Id}");
            }
            
            
        }
    }
}
