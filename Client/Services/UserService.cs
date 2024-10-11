using Messenger.Shared.DTO;
using Messenger.Shared.Models;
using Microsoft.JSInterop;
using System.Net.Http.Json;
using Microsoft.JSInterop;
using System.Text.Json;
namespace Messenger.Client.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;

        public UserService(HttpClient httpClient, IJSRuntime jsRuntime)
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
        }

        public async Task<List<Conv>> GetConv(string apiname)
        {
            return await _httpClient.GetFromJsonAsync<List<Conv>>(apiname);
        }

        public async Task SendMessage(Conv message,string api)
        {
            try
            {
                // Send the message to the server
                var response = await _httpClient.PostAsJsonAsync(api, message);

                // Check if the response indicates success
                if (response.IsSuccessStatusCode)
                {
                    // Optionally, you can log success or perform additional actions
                    Console.WriteLine("Message sent successfully.");
                }
                else
                {
                    // Handle the failure case
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Failed to send message: {response.StatusCode} - {errorMessage}");
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the HTTP request
                Console.WriteLine($"An error occurred while sending the message: {ex.Message}");
            }
        }

        public async Task Seen(string api)
        {
            try
            {
                // Send the request to mark messages as seen
                var response = await _httpClient.PutAsJsonAsync<object>(api, null); // Specify type explicitly

                // Check if the response indicates success
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Messages marked as seen successfully.");
                }
                else
                {
                    // Handle the failure case
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Failed to mark messages as seen: {response.StatusCode} - {errorMessage}");
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the HTTP request
                Console.WriteLine($"An error occurred while marking messages as seen: {ex.Message}");
            }
        }

        public async Task<bool> Login(UserDto userDto, string api)
        {
            try
            {
                // Send the login request to the server
                var response = await _httpClient.PostAsJsonAsync(api, userDto);

                if (response.IsSuccessStatusCode)
                {
                    // Deserialize the User object from the server response
                    var result = await response.Content.ReadFromJsonAsync<User>();

                    // Store the User object as a JSON string in sessionStorage
                    string userJson = JsonSerializer.Serialize(result);
                    await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "loggedInUser", userJson);

                    Console.WriteLine("Login successful.");
                    return true;
                }
                else
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Login failed: {response.StatusCode} - {errorMessage}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred during login: {ex.Message}");
                return false;
            }
        }


        public async Task<User> GetLoggedInUser()
        {
            // Retrieve the user data from sessionStorage
            string userJson = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "loggedInUser");

            if (!string.IsNullOrEmpty(userJson))
            {
                // Deserialize the JSON string back into a User object
                return JsonSerializer.Deserialize<User>(userJson);
            }
            return null;
        }
        public async Task<List<User>> Friends(string apiname)
        {
            return await _httpClient.GetFromJsonAsync<List<User>>(apiname);
        }

        public async Task<List<User>> NoFriends(string apiname)
        {
            return await _httpClient.GetFromJsonAsync<List<User>>(apiname);
        }

        public async Task SendFriendReq(string apiname, FriendRequestDto friendRequest)
        {

            var response = await _httpClient.PostAsJsonAsync(apiname, friendRequest);
           
        }

        public async Task<List<FriendUserDto>> GetAllUsers(string apiname)
        {
            return await _httpClient.GetFromJsonAsync<List<FriendUserDto>>(apiname);
        }

        public async Task<bool> Accept(string apiname)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync<object>(apiname, null);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine($"Failed to call API. Status Code: {response.StatusCode}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception while calling API: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> Decline(string apiname)
        {
            try
            {
                // Use DeleteAsync to make a DELETE request
                var response = await _httpClient.DeleteAsync(apiname); // Corrected method to DeleteAsync

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine($"Failed to call API. Status Code: {response.StatusCode}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception while calling API: {ex.Message}");
                return false;
            }
        }





    }
}
