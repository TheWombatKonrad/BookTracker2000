using BookTrackersApi.Authorization;
using BookTrackersApi.DatabaseContext;
using BookTrackersApi.Models.Users;
using BookTrackersApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookTrackersApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            var user = _userService.GetUserView(id);
            return Ok(user);
        }

        [HttpGet("current")]
        public IActionResult GetCurrentUser()
        {
            var user = _userService.GetCurrentUser();
            return Ok(user);
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateUserRequest model)
        {
            _userService.Update(id, model);
            return Ok(new { message = "User updated successfully" });
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = _userService.Authenticate(model);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register(RegisterUserRequest model)
        {
            _userService.Register(model);
            return Ok(new { message = "Registration successful" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _userService.Delete(id);
            return Ok(new { message = "User deleted successfully" });
        }

        [HttpDelete("current")]
        public IActionResult DeleteCurrentUser()
        {
            _userService.DeleteCurrentUser();
            return Ok(new { message = "User deleted successfully" });
        }

        //----Books
        [HttpDelete("{userId}/books/{bookId}")]
        public IActionResult DeleteUserBook(int userId, int bookId)
        {
            _userService.DeleteBookFromUser(userId, bookId);
            return Ok(new { message = "Book deleted successfully" });
        }

        [HttpDelete("current/books/{bookId}")]
        public IActionResult DeleteCurrentUserBook(int bookId)
        {
            _userService.DeleteBookFromCurrentUser(bookId);
            return Ok(new { message = "Book deleted successfully" });
        }
    }
}
