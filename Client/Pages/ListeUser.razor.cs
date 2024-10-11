using Messenger.Client.Services;
using Messenger.Shared.DTO;
using Messenger.Shared.Models;
using Microsoft.AspNetCore.Components;
using System.Threading;

namespace Messenger.Client.Pages
{
    public partial class ListeUser
    {
        public string id1 { get; set; }
        List<FriendUserDto> users { get; set; } = new List<FriendUserDto>();

        [Inject]
        public IUserService _user { get; set; }

        [Inject]
        public IMainService<User> _user2 { get; set; }
        private CancellationTokenSource cancellationTokenSource;

        protected override async Task OnInitializedAsync()
        {
            var loggedInUser = await _user.GetLoggedInUser();
            id1 = loggedInUser.Id; // Set logged-in user id
            await RefreshUsersAsync(); // Fetch initial users
            cancellationTokenSource = new CancellationTokenSource();
            _ = Task.Run(() => PollForNewUsersEverySecond(cancellationTokenSource.Token)); // Start polling
        }

        private async Task PollForNewUsersEverySecond(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await RefreshUsersAsync(); // Fetch users
                await Task.Delay(1000); // Wait for 1 second before the next call
            }
        }

        private async Task RefreshUsersAsync()
        {
            try
            {
                var loggedInUser = await _user.GetLoggedInUser();
                id1 = loggedInUser.Id; // Set logged-in user id again
                users = await _user.GetAllUsers($"/api/Users/GetAllUsers/{id1}");
                Console.WriteLine($"Fetched {users.Count} users."); // Debugging log
                StateHasChanged(); // Force UI update
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while fetching users: {ex.Message}");
            }
        }


        public async Task SendFR(string recid)
        {
            var loggedInUser = await _user.GetLoggedInUser();
            id1 = loggedInUser.Id; // Sender ID
            FriendRequestDto friendreq = new FriendRequestDto()
            {
                SenderId = id1,
                ReceiverId = recid,
            };
            await _user.SendFriendReq($"/api/Users/SendFriendReq/", friendreq);
        }

        public async Task AcceptFR(string id)
        {
            Console.WriteLine("AcceptFR."); // Log for debugging
            try
            {
                var response = await _user.Accept($"/api/Users/AcceptFriendReq/{id}");
                if (response)
                {
                    Console.WriteLine("Friend request accepted successfully.");
                }
                else
                {
                    Console.WriteLine("Failed to accept friend request.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public async Task DeclineFR(string id)
        {
            Console.WriteLine("DeclineFR."); // Log for debugging
            try
            {
                var response = await _user.Decline($"/api/Users/DeclineFriendReq/{id}");
                if (response)
                {
                    Console.WriteLine("Friend request declined successfully.");
                }
                else
                {
                    Console.WriteLine("Failed to decline friend request.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
