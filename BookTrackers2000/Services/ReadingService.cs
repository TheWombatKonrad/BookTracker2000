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

    //returns all readings readingview
    public IEnumerable<ReadingView> GetAll()
    {
        var readingViews = new List<ReadingView>();

        foreach (var reading in _context.Readings)
        {
            readingViews.Add(CreateReadingView(reading));
        }

        return readingViews;
    }

    //returns specific readings readingview
    public ReadingView GetById(int id)
    {
        var reading = getReading(id);
        return CreateReadingView(reading);
    }

    //gets all readings by the current user
    //user cant actually be null bc the method requires authentication
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

    //creates a reading, sets pages, sets userbook (contains two methods)
    public void Register(RegisterReadingRequest model)
    {
        var reading = _mapper.Map<Reading>(model); //adds the pages read
        reading.UserBook = SetUserBook(model.BookId);
         
        _context.Readings.Add(reading);
        _context.SaveChanges();

        UserBook SetUserBook(int bookId)
        {
            //the http request is not anonymous so there should always be a user
            var user = (User)_httpContextAccessor.HttpContext.Items["User"];

            var userBook = user.Books.FirstOrDefault(x => x.Book.Id == model.BookId);

            //if the book is not in the users list, it is first added
            if (userBook == null)
            {
                userBook = CreateUserBook(model.BookId, user);
            }

            //calculates the total pages read
            userBook.TotalPagesRead += model.PagesRead;

            //sets the book to finished if applicable
            if (userBook.TotalPagesRead >= userBook.Book.Pages)
            {
                userBook.Finished = true;
                userBook.TotalPagesRead = userBook.Book.Pages;
            }

            return userBook;
        }

        UserBook CreateUserBook(int bookId, User user)
        {
            var book = _context.Books.Find(model.BookId);

            if (book == null)
                throw new KeyNotFoundException("Book not found");

            var userBook = new UserBook();

            userBook.Book = book;
            userBook.User = user;

            return userBook;
        }
    }

    public void Update(int id, UpdateReadingRequest model)
    {
        var reading = getReading(id);

        //makes sure reading is never more than how many pages remain
        var temp = reading.UserBook.Book.Pages - reading.UserBook.TotalPagesRead;

        if (model.PagesRead > temp)
        {
            model.PagesRead = temp;
        }

        //corrects total pages read
        reading.UserBook.TotalPagesRead = 
            reading.UserBook.TotalPagesRead + model.PagesRead - reading.PagesRead;

        //makes sure the userbook is correct
        if (reading.UserBook.TotalPagesRead >= reading.UserBook.Book.Pages)
        {
            reading.UserBook.Finished = true;
            reading.UserBook.TotalPagesRead = reading.UserBook.Book.Pages;
        }
        else
            reading.UserBook.Finished = false;

        _mapper.Map(model, reading);

        _context.Readings.Update(reading);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var reading = getReading(id);

        //updates the pages read and if the book is finished
        UpdateUserBook(reading);

        _context.Readings.Remove(reading);
        _context.SaveChanges();

        void UpdateUserBook(Reading reading)
        {
            var userBook = reading.UserBook;

            userBook.TotalPagesRead -= reading.PagesRead;
            userBook.Finished = false;
        }
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

    //returns a readingview, containing a userbookview
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