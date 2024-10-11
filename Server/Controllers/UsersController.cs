using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Messenger.Server.Models;
using Messenger.Shared.Models;
using Messenger.Server.Repositories.Interfaces;
using Messenger.Shared.DTO;
using NuGet.Protocol.Plugins;

namespace Messenger.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IRepository<User> _user;
        private readonly IUserRepository _user2;

        public UsersController(IRepository<User> user, IUserRepository user2)
        {
            _user = user;
            _user2 = user2;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _user.GetAllAsync();
            if (users == null || !users.Any())
            {
                return NotFound();
            }
            return Ok(users);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            var user = await _user.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            await _user.AddAsync(user);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(string id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            try
            {
                await _user.UpdateAsync(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _user.GetByIdAsync(id) == null)
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _user.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _user.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> SendFriendReq(FriendRequestDto friendRequest)
        {
            try
            {
                // Call the repository method to send the friend request
                await _user2.SendFriendReq(friendRequest.SenderId, friendRequest.ReceiverId); // Added 'await'
                return NoContent(); // Return 204 No Content on success
            }
            catch (Exception ex)
            {
                // Return a bad request with the error message in case of failure
                return BadRequest($"An error occurred while sending the friend request: {ex.Message}");
            }
        }

        [HttpPut("{id}")] // Adjust the route if needed
        public async Task<IActionResult> AcceptFriendReq(int id )
        {
            try
            {
                // Call the repository method to accept the friend request
                await _user2.AcceptFriendReq(id); // Added 'await'
                return NoContent(); // Return 204 No Content on success
            }
            catch (Exception ex)
            {
                // Return a bad request with the error message in case of failure
                return BadRequest($"An error occurred while accepting the friend request: {ex.Message}");
            }
        }
        [HttpDelete("{id}")] // Adjust the route if needed
        public async Task<IActionResult> DeclineFriendReq(int id )
        {
            try
            {
                // Call the repository method to accept the friend request
                await _user2.DeclinedFriendReq(id); // Added 'await'
                return NoContent(); // Return 204 No Content on success
            }
            catch (Exception ex)
            {
                // Return a bad request with the error message in case of failure
                return BadRequest($"An error occurred while accepting the friend request: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> SendMsg(Conv conv)
        {
            try
            {
                // Call the repository method to send the message
                await _user2.SendMsg(conv); // Await the repository call
                return NoContent(); // Return 204 No Content on success
            }
            catch (DbUpdateException ex)
            {
                // Return a bad request with the error message in case of failure
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Friends(string id="1")
        {
            try
            {
                // Call the repository method to send the message
                
                return Ok(await _user2.Friends(id)); // Return 204 No Content on success
            }
            catch (DbUpdateException ex)
            {
                // Return a bad request with the error message in case of failure
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> NoFriends(string id = "1")
        {
            try
            {
                // Call the repository method to send the message

                return Ok(await _user2.NoFriends(id)); // Return 204 No Content on success
            }
            catch (DbUpdateException ex)
            {
                // Return a bad request with the error message in case of failure
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{idemet}/{idrec}")]
        public async Task<ActionResult<IEnumerable<Conv>>> Conversation(string idemet, string idrec)
        {
            try
            {
                // Call the repository method to get the conversation
                return Ok(await _user2.Conversation(idemet, idrec)); // Return the conversation data
            }
            catch (DbUpdateException ex)
            {
                // Return a bad request with the error message in case of failure
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{idemet}/{idrec}")]
        public async Task<IActionResult> Seen(string idemet,string idrec)
        {
            try
            {
                // Call the Seen method to update the conversations
                await _user2.Seen(idemet,idrec);
                return Ok(new { message = "Messages marked as seen." });
            }
            catch (Exception ex)
            {
                // Handle exceptions and return an appropriate response
                return StatusCode(500, new { message = ex.Message });
            }
        }


        [HttpPost]
        public async Task<IActionResult> Login(UserDto userDto)
        {
            try
            {
                //okay
                if (await _user2.Login(userDto) != null)
                    return Ok(await _user2.Login(userDto));
                else return BadRequest();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<FriendUserDto>>> GetAllUsers(string id)
        {
            var users = await _user2.GetAllUsers(id);
            if (users == null || !users.Any())
            {
                return NotFound();
            }
            return Ok(users);
        }





    }

}
