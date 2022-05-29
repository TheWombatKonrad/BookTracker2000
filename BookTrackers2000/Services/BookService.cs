namespace BookTrackersApi.Services;

using AutoMapper;
using BookTrackersApi.DatabaseContext;
using BookTrackersApi.Entities;
using BookTrackersApi.Models.Books;
using BookTrackersApi.Views;

public interface IBookService
{
    IEnumerable<BookView> GetAll();
    BookView GetBook(int id);
    void Register(RegisterBookRequest model);
    void Update(int id, UpdateBookRequest model);
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

    public IEnumerable<BookView> GetAll()
    {
        var books = new List<BookView>();

        foreach(var book in _context.Books)
        {
            books.Add(createBookView(book));
        }

        return books;
    }

    public BookView GetBook(int id)
    {
        var book = getById(id);
        return createBookView(book);
    }

    public void Register(RegisterBookRequest model)
    {
        var book = _mapper.Map<Book>(model);
        var author = _context.Authors.Find(model.AuthorId);
        book.Authors.Add(author);

        _context.Books.Add(book);
        _context.SaveChanges();
    }

    public void Update(int id, UpdateBookRequest model)
    {
        var book = getById(id);
        var author = _context.Authors.Find(model.AuthorId);

        if ((book.Authors.FirstOrDefault(x => x.Id == model.AuthorId) is null) && author != null)
        {
            book.Authors.Add(author);
        }

        if (model.Pages != 0)
        {
            book.Pages = model.Pages;
        }

        if (model.Title != null)
        {
            book.Title = model.Title;
        }


        _context.Books.Update(book);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var book = getById(id);
        _context.Books.Remove(book);
        _context.SaveChanges();
    }

    public void RemoveAuthor(int bookId, int authorId)
    {
        var book = getById(bookId);

        if (book.Authors.Count < 2)
        {
            throw new InvalidOperationException("There must be at least one author of the book.");
            return;
        }

        var author = book.Authors.FirstOrDefault(x => x.Id == authorId);

        if (author == null)
        {
            throw new KeyNotFoundException("Author not found.");
            return;
        }

        book.Authors.Remove(author);
        _context.SaveChanges();
    }

    //********************************
    //Helper Methods
    //********************************

    private Book getById(int id)
    {
        var book = _context.Books.Find(id);
        if (book == null) throw new KeyNotFoundException("Book not found");
        return book;
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