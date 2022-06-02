using BookTrackersApi.Authorization;
using BookTrackersApi.DatabaseContext;
using BookTrackersApi.Models.Books;
using BookTrackersApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookTrackersApi.Controllers
{
    [Authorize]
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAll()
        {
            var books = _bookService.GetAll();
            return Ok(books);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult GetBook(int id)
        {
            var book = _bookService.GetBook(id);
            return Ok(book);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateBookRequest model)
        {
            _bookService.Update(id, model);
            return Ok(new { message = "Book updated successfully" });
        }

        [HttpPut("{bookId}/authors/add")]
        public IActionResult AddAuthor(int bookId, AddAuthorRequest model)
        {
            _bookService.AddAuthor(bookId, model);
            return Ok(new { message = "Author added successfully" });
        }

        [HttpPost]
        public IActionResult Register(RegisterBookRequest model)
        {
            _bookService.Register(model);
            return Ok(new { message = "Book registration successful" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _bookService.Delete(id);
            return Ok(new { message = "Book deleted successfully" });
        }

        [HttpDelete("{bookId}/authors/remove/{authorId}")]
        public IActionResult RemoveAuthor(int bookId, int authorId)
        {
            _bookService.RemoveAuthor(bookId, authorId);
            return Ok(new { message = "Author was removed successfully!" });
        }
    }
}
