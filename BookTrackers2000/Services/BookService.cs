namespace BookTrackersApi.Services;

using AutoMapper;
using BookTrackersApi.DatabaseContext;
using BookTrackersApi.Entities;
using BookTrackersApi.Helpers;
using BookTrackersApi.Models.Books;
using BookTrackersApi.Views;

public interface IBookService
{
    IEnumerable<BookView> GetAll();
    BookView GetBook(int id);
    void Register(RegisterBookRequest model);
    void Update(int id, UpdateBookRequest model);
    void AddAuthor(int bookId, AddAuthorRequest model);
    void Delete(int id);
    void RemoveAuthor(int bookId, int authorId);
}

public class BookService : IBookService
{
    private SqliteDataContext _context;
    private readonly IMapper _mapper;

    public BookService(
        SqliteDataContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    //returns all books bookview - no userinfo
    public IEnumerable<BookView> GetAll()
    {
        var books = new List<BookView>();

        foreach(var book in _context.Books)
        {
            books.Add(createBookView(book));
        }

        return books;
    }

    //returns a specific books bookview via id
    public BookView GetBook(int id)
    {
        var book = getBookById(id);
        return createBookView(book);
    }

    //registers a book - the author must already exist
    public void Register(RegisterBookRequest model)
    {
        var book = _mapper.Map<Book>(model);

        var author = _context.Authors.Find(model.AuthorId);
        if (author == null) throw new KeyNotFoundException("Author not found");

        book.Authors.Add(author);

        _context.Books.Add(book);
        _context.SaveChanges();
    }

    //updates the books info
    //removes the previous author if a new author is added
    //didn't use automapper bc it didn't work properly
    public void Update(int bookId, UpdateBookRequest model)
    {
        var book = getBookById(bookId);

        //if an authorId has been entered, the author is found
        //if it is found it is added (otherwise an error is thrown)
        if(model.AuthorId != 0)
        {
            var author = getAuthorById(model.AuthorId);

            book.Authors.Clear();
            book.Authors.Add(author);
        }

        if (model.Pages != 0)
        {
            book.Pages = model.Pages;
        }

        if (!string.IsNullOrEmpty(model.Title))
        {
            book.Title = model.Title;
        }

        _context.Books.Update(book);
        _context.SaveChanges();
    }

    //adds a new author to a book
    public void AddAuthor(int bookId, AddAuthorRequest model)
    {
        var book = getBookById(bookId);
        var author = getAuthorById(model.AuthorId);

        if (book.Authors.Contains(author))
            throw new AppException("That author has already been added");

        book.Authors.Add(author);

        _context.Books.Update(book);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var book = getBookById(id);
        _context.Books.Remove(book);
        _context.SaveChanges();
    }

    //if there is at least 2 authors, an author is removed
    public void RemoveAuthor(int bookId, int authorId)
    {
        var book = getBookById(bookId);

        if (book.Authors.Count < 2)
            throw new InvalidOperationException("There must be at least one author of the book.");

        var author = getAuthorById(authorId);

        book.Authors.Remove(author);
        _context.SaveChanges();
    }

    //********************************
    //Helper Methods
    //********************************

    private Book getBookById(int id)
    {
        var book = _context.Books.Find(id);
        if (book == null) throw new KeyNotFoundException("Book not found");
        return book;
    }

    private Author getAuthorById(int id)
    {
        var author = _context.Authors.Find(id);
        if (author == null) throw new KeyNotFoundException("Author not found");
        return author;
    }

    private BookView createBookView(Book book)
    {
        return new BookView
        {
            Id = book.Id,
            Title = book.Title,
            Authors = book.Authors,
            Pages = book.Pages
        };
    }
}