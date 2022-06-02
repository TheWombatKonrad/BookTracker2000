using BookTrackersApi.Authorization;
using BookTrackersApi.DatabaseContext;
using BookTrackersApi.Models.Authors;
using BookTrackersApi.Models.Books;
using BookTrackersApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookTrackersApi.Controllers
{
    [Authorize]
    [Route("api/authors")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private IAuthorService _authorService;

        public AuthorsController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAll()
        {
            var authors = _authorService.GetAll();
            return Ok(authors);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult GetAuthor(int id)
        {
            var author = _authorService.GetAuthor(id);
            return Ok(author);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateAuthorRequest model)
        {
            _authorService.Update(id, model);
            return Ok(new { message = "Author updated successfully" });

        }

        [HttpPost]
        public IActionResult Register(RegisterAuthorRequest model)
        {
            var author = _authorService.Register(model);
            return Ok(author);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _authorService.Delete(id);
            return Ok(new { message = "Author deleted successfully" });
        }


    }
}
