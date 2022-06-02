namespace BookTrackersApi.Services;

using AutoMapper;
using BCrypt.Net;
using BookTrackersApi.Authorization;
using BookTrackersApi.DatabaseContext;
using BookTrackersApi.Entities;
using BookTrackersApi.Helpers;
using BookTrackersApi.Models.Users;
using BookTrackersApi.Views;

public interface IUserService
{
    AuthenticateResponse Authenticate(AuthenticateRequest model);
    IEnumerable<UserView> GetAll();
    User GetUser(int id);
    UserView GetUserView(int id);
    UserView GetCurrentUser();
    void Register(RegisterUserRequest model);
    void Update(int id, UpdateUserRequest model);
    void Delete(int id);
    void DeleteCurrentUser();
    void DeleteBookFromUser(int userId, int bookId);
    void DeleteBookFromCurrentUser(int bookdId);
}

public class UserService : IUserService
{
    private SqliteDataContext _context;
    private IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private IJwtUtils _jwtUtils;

    public UserService(
        SqliteDataContext context,
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        IJwtUtils jwtUtils)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _jwtUtils = jwtUtils;
    }

    public AuthenticateResponse Authenticate(AuthenticateRequest model)
    {
        var user = _context.Users.SingleOrDefault(x => x.Username == model.Username);

        // validate
        if (user == null || !BCrypt.Verify(model.Password, user.PasswordHash))
            throw new AppException("Username or password is incorrect");

        // authentication successful
        var response = _mapper.Map<AuthenticateResponse>(user);
        response.Token = _jwtUtils.GenerateToken(user);

        return response;
    }

    public IEnumerable<UserView> GetAll()
    {
        List<UserView> users = new List<UserView>();

        foreach(var user in _context.Users)
        {
            users.Add(createUserView(user));
        }

        return users;
    }

    //important for getting user from httpcontext
    public User GetUser(int id)
    {
        return getById(id);
    }

    public UserView GetUserView(int id)
    {
        var user = getById(id);
        return createUserView(user);
    }

    public UserView GetCurrentUser()
    {
        //doesn't allow anonynous so didn't add exception...
        var user = (User)_httpContextAccessor.HttpContext.Items["User"];
        return createUserView(user);
    }

    public void Register(RegisterUserRequest model)
    {
        // validate
        if (_context.Users.Any(x => x.Username == model.Username))
            throw new AppException("Username '" + model.Username + "' is already taken");

        // map model to new user object
        var user = _mapper.Map<User>(model);

        // hash password
        user.PasswordHash = BCrypt.HashPassword(model.Password);

        // save user
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public void Update(int id, UpdateUserRequest model)
    {
        var user = getById(id);

        // validate
        if (model.Username != user.Username && _context.Users.Any(x => x.Username == model.Username))
            throw new AppException("Username '" + model.Username + "' is already taken");

        // hash password if it was entered
        if (!string.IsNullOrEmpty(model.Password))
            user.PasswordHash = BCrypt.HashPassword(model.Password);

        // copy model to user and save
        _mapper.Map(model, user);
        _context.Users.Update(user);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var user = getById(id);

        _context.Users.Remove(user);
        _context.SaveChanges();
    }

    public void DeleteCurrentUser()
    {
        var user = (User)_httpContextAccessor.HttpContext.Items["User"];

        if (user == null) throw new KeyNotFoundException("User not found");

        _context.Users.Remove(user);
        _context.SaveChanges();
    }

    public void DeleteBookFromUser(int userId, int bookId)
    {
        var userBook = _context.Users.Find(userId)
            .Books.FirstOrDefault(x => x.Book.Id == bookId);

        if (userBook == null) 
            throw new KeyNotFoundException("Either the user or the book was not found.");

        _context.Remove(userBook);
        _context.SaveChanges();
    }

    public void DeleteBookFromCurrentUser(int bookId)
    {
        var user = (User)_httpContextAccessor.HttpContext.Items["User"];
        var userBook = user.Books.FirstOrDefault(x => x.Book.Id == bookId);

        if (userBook == null) throw new KeyNotFoundException("Book not found");

        _context.Remove(userBook);
        _context.SaveChanges();
    }

    //********************************
    //Helper Methods
    //********************************

    private User getById(int id)
    {
        var user = _context.Users.Find(id);
        if (user == null) throw new KeyNotFoundException("User not found");
        return user;
    }

    //from a list of userbooks, returns a list of userBookViews
    private IEnumerable<UserBookView> getBookList(IEnumerable<UserBook> userBooks)
    {
        var bookList = new List<UserBookView>();

        foreach (var userBook in userBooks)
        {
                var book = new UserBookView
                {
                    Id = userBook.Book.Id,
                    Title = userBook.Book.Title,
                    Authors = userBook.Book.Authors,
                    Finished = userBook.Finished,
                    TotalPagesRead = userBook.TotalPagesRead,
                    TotalPages = userBook.Book.Pages,
                    Readings = userBook.Readings
                };

                bookList.Add(book);
        }

        return bookList;
    }

    private UserView createUserView(User _user)
    {
        return new UserView
        {
            Id = _user.Id,
            Username = _user.Username,
            Email = _user.Email,
            BookList = getBookList(_user.Books)
        };
    }
}