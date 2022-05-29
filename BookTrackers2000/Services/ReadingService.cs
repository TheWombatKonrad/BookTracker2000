namespace BookTrackersApi.Services;

using AutoMapper;
using BookTrackersApi.DatabaseContext;
using BookTrackersApi.Entities;
using BookTrackersApi.Models.Readings;
using BookTrackersApi.Views;


public interface IReadingService
{
    IEnumerable<ReadingView> GetAll();
    ReadingView GetById(int id);
    IEnumerable<ReadingView> GetByCurrentUser();
    void Register(RegisterReadingRequest model);
    void Update(int id, UpdateReadingRequest model);
    void Delete(int id);
}

public class ReadingService : IReadingService
{
    private SqliteDataContext _context;
    private readonly IMapper _mapper;
    private IHttpContextAccessor _httpContextAccessor;

    public ReadingService(
        SqliteDataContext context,
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
    }

    public IEnumerable<ReadingView> GetAll()
    {
        var readingViews = new List<ReadingView>();

        foreach (var reading in _context.Readings)
        {
            readingViews.Add(CreateReadingView(reading));
        }

        return readingViews;
    }

    public ReadingView GetById(int id)
    {
        var reading = getReading(id);
        return CreateReadingView(reading);
    }

    public IEnumerable<ReadingView> GetByCurrentUser()
    {
        var user = (User)_httpContextAccessor.HttpContext.Items["User"];
        var readingViews = new List<ReadingView>();

        foreach(var book in user.Books)
        {
            foreach(var reading in book.Readings)
            { 
                readingViews.Add(CreateReadingView(reading)); 
            }
        }

        return readingViews;
    }

    public void Register(RegisterReadingRequest model)
    {
        var reading = _mapper.Map<Reading>(model);

        var user = (User)_httpContextAccessor.HttpContext.Items["User"];
        //the http request is not anonymous so there should always be a user

        var userBook = user.Books.FirstOrDefault(x => x.Book.Id == model.BookId);

        if (userBook == null)
        {
            reading.UserBook.Book = _context.Books.Find(model.BookId);

            if (reading.UserBook.Book == null) throw new KeyNotFoundException("Book not found");

            reading.UserBook.User = user;
        }

        else
            reading.UserBook = userBook;

        reading.UserBook.TotalPagesRead += model.PagesRead;

        if (reading.UserBook.TotalPagesRead >= reading.UserBook.Book.Pages)
        {
            reading.UserBook.Finished = true;
            reading.UserBook.TotalPagesRead = reading.UserBook.Book.Pages;
        }
            
        _context.Readings.Add(reading);
        _context.SaveChanges();
    }

    public void Update(int id, UpdateReadingRequest model)
    {
        var reading = getReading(id);

        _mapper.Map(model, reading);
        _context.Readings.Update(reading);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var reading = getReading(id);
        _context.Readings.Remove(reading);
        _context.SaveChanges();
    }

    //********************************
    //Helper Methods
    //********************************

    private Reading getReading(int id)
    {
        var reading = _context.Readings.Find(id);
        if (reading == null) throw new KeyNotFoundException("Reading not found");
        return reading;
    }

    private ReadingView CreateReadingView(Reading reading)
    {
        var userBook = reading.UserBook;
        var user = reading.UserBook.User;

        var _userBook = new UserBookView
        {
            Id = userBook.Id,
            Title = userBook.Book.Title,
            Authors = userBook.Book.Authors,
            Finished = userBook.Finished,
            TotalPages = userBook.Book.Pages,
            TotalPagesRead = userBook.TotalPagesRead
        };

        return new ReadingView
        {
            ReadingId = reading.Id,
            UserId = user.Id,
            PagesRead = reading.PagesRead,
            Book = _userBook,
            
            
        };
    }
}