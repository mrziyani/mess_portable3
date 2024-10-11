using Messenger.Server.Models;
using Messenger.Server.Repositories.Interfaces;
using Messenger.Shared.DTO;
using Messenger.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Server.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AcceptFriendReq(int id)
        {
            try
            {
                // Find the friend request by ID
                Friend friend = await _context.Set<Friend>().FindAsync(id);

                if (friend == null)
                {
                    throw new Exception("Friend request not found.");
                }

                // Update the 'Etat' to true (accepted)
                friend.Etat = true;

                // Save changes to the database
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log and handle the error appropriately
                Console.WriteLine($"An error occurred while accepting the friend request: {ex.Message}");
                throw new Exception("An error occurred while accepting the friend request.", ex);
            }
        }



        public async Task DeclinedFriendReq(int id)
        {
            try
            {
                // Find the friend request by ID
                Friend friend = await _context.Set<Friend>().FindAsync(id);

                if (friend != null)
                {
                    _context.Remove(friend);
                    await _context.SaveChangesAsync();
                }

                if (friend == null)
                {
                    throw new Exception("Friend request not found.");
                }


                // Save changes to the database
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log and handle the error appropriately
                Console.WriteLine($"An error occurred while declining the friend request: {ex.Message}");
                throw new Exception("An error occurred while declining the friend request.", ex);
            }
        }

        public async Task SendFriendReq(string senderId, string receiverId)
        {
            try
            {


                // Create a new Friend object
                Friend friend = new Friend
                {
                    IdEmet = senderId,   // Set the sender's ID
                    IdRec = receiverId,  // Set the receiver's ID
                    Etat = false         // Default state for a new friend request
                };

                // Add the friend request to the database
                await _context.Set<Friend>().AddAsync(friend);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                // Handle database-specific exceptions (e.g., foreign key violations, constraints, etc.)
                Console.WriteLine($"Database error: {dbEx.Message}");
                throw new Exception("There was a problem saving the friend request to the database.", dbEx);
            }
            catch (Exception ex)
            {
                // Handle general exceptions
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw new Exception("An unexpected error occurred while sending the friend request.", ex);
            }
        }

        //SendMsg
        public async Task SendMsg(Conv conv)
        {
            try
            {
                await _context.Set<Conv>().AddAsync(conv);
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateException dbEx)
            {
                // Handle database-specific exceptions
                Console.WriteLine($"Database error: {dbEx.InnerException?.Message}"); // Log inner exception
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                throw;
            }
        }

        public async Task Seen(string idemet, string idrec)
        {
            try
            {
                // Find all conversations where idrec matches the provided id
                var convsToUpdate = await _context.Set<Conv>()
                    .Where(conv => conv.IdEmet == idrec && conv.IdRec == idemet)
                    //.Where(conv => conv.IdEmet == "1" && conv.IdRec == id)// Assuming `IdRec` is the correct property name
                    .ToListAsync();

                if (convsToUpdate == null || !convsToUpdate.Any())
                {
                    Console.WriteLine($"No conversations found for idrec: {idemet}");
                    return; // Optionally handle the case where no records are found
                }

                // Update the necessary properties for each conversation
                foreach (var conv in convsToUpdate)
                {
                    // Assuming you want to set some property to indicate the message has been seen
                    conv.Etat = 1; // Update the `Etat` property or any other property as needed
                }

                // Save changes to the database
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                // Handle database-specific exceptions
                Console.WriteLine($"Database error: {dbEx.InnerException?.Message}"); // Log inner exception
                throw;
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                throw;
            }
        }
        public async Task<List<User>> Friends(string id)
        {
            try
            {
                // Retrieve all friends where idemet or idrec matches the provided id and etat is true
                var friendUsers = await _context.Set<Friend>()
                    .Where(friend => (friend.IdEmet == id || friend.IdRec == id) && friend.Etat == true)
                    .Select(friend => friend.IdEmet == id ? friend.Receiver : friend.Sender) // Select the friend user
                    .ToListAsync();

                return friendUsers; // Return the list of users who are friends
            }
            catch (Exception ex)
            {
                // Handle any potential exceptions
                Console.WriteLine($"An error occurred while retrieving friends: {ex.Message}");
                throw; // Rethrow the exception for higher-level handling if needed
            }
        }


        public async Task<List<User>> NoFriends(string id)
        {
            try
            {
                // Retrieve all friend usersdddd
                var friendUsers = await _context.Set<Friend>()
                    .Where(friend => (friend.IdEmet == id || friend.IdRec == id))
                    .Select(friend => friend.IdEmet == id ? friend.Receiver : friend.Sender) // Select the friend userdfsdfsqdf
                    .ToListAsync();

                // Retrieve all users except the current user
                var allUsers = await _context.Set<User>()
                    .Where(user => user.Id != id) // Exclude the current user
                    .ToListAsync();

                // Exclude friend users from all users
                var nonFriendUsers = allUsers
                    .Where(user => !friendUsers.Any(friendUser => friendUser.Id == user.Id))
                    .ToList();

                return nonFriendUsers; // Return the list of users who are not friends, excluding the current user
            }
            catch (Exception ex)
            {
                // Handle any potential exceptions
                Console.WriteLine($"An error occurred while retrieving non-friend users: {ex.Message}");
                throw; // Rethrow the exception for higher-level handling if needed
            }
        }

        public async Task<List<Conv>> Conversation(string idemet, string idrec)
        {


            var conversations = await _context.Set<Conv>()
                .Where(conv => (conv.IdEmet == idemet && conv.IdRec == idrec) || (conv.IdEmet == idrec && conv.IdRec == idemet))
                .ToListAsync();

            return conversations;
        }

        public async Task<User> Login(UserDto userDto)
        {
            // Use FirstOrDefaultAsync to get a single user or null if not found
            User user = await _context.Set<User>()
                .FirstOrDefaultAsync(user => user.Email == userDto.Email && user.Mdp == userDto.Mdp);
            if (user == null) { return null; }
            return user;
        }


        public async Task<IEnumerable<FriendUserDto>> GetAllUsers(string userId)
        {
            // First, get all users excluding the specified user
            var users = await _context.Users
                .Where(user => user.Id != userId)
                .ToListAsync();

            // Then, get all friend relations for the specified user
            var friends = await _context.Friends
                .Where(friend => friend.IdEmet == userId || friend.IdRec == userId)
                .ToListAsync();

            // Now, project the results manually
            var result = users.Select(user =>
            {
                // Find the matching friend relation where the current user is either the sender or receiver
                var friendRelation = friends.FirstOrDefault(friend => friend.IdEmet == user.Id || friend.IdRec == user.Id);

                return new FriendUserDto
                {
                    Id = friendRelation?.Id.ToString(), // Assign the friend relationship Id (default to 0 if not found)
                    IdUser = user.Id,
                    Idemet = friendRelation?.IdEmet, // Set Idemet from the friend relation
                    UserName = user.Nom,
                    UserPrenom = user.Prenom,
                    etat = friendRelation?.Etat // Set the friend request status (etat)
                };
            }).ToList();

            return result;
        }



    }
}
