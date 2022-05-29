namespace BookTrackersApi.Services;

using AutoMapper;
using BookTrackersApi.DatabaseContext;
using BookTrackersApi.Entities;
using BookTrackersApi.Helpers;
using BookTrackersApi.Models.Authors;
using BookTrackersApi.Views;

public interface IAuthorService
{
    IEnumerable<AuthorView> GetAll();
    AuthorView GetAuthor(int id);
    Author Register(RegisterAuthorRequest model);
    void Update(int id, UpdateAuthorRequest model);
    void Delete(int id);
}

public class AuthorService : IAuthorService
{
    private SqliteDataContext _context;
    private readonly IMapper _mapper;

    public AuthorService(
        SqliteDataContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public IEnumerable<AuthorView> GetAll()
    {
        List<AuthorView> authors = new List<AuthorView>();

        foreach(var author in _context.Authors)
        {
            authors.Add(createAuthorView(author));
        }

        return authors;

    }

    public AuthorView GetAuthor(int id)
    {
        var author = getById(id);
        return createAuthorView(author);
    }

    public Author Register(RegisterAuthorRequest model)
    {
        var author = _mapper.Map<Author>(model);
        var dbAuthors = _context.Authors;

        foreach(var dbAuthor in dbAuthors)
        {
            if ((author.LastName == dbAuthor.LastName) && (author.FirstName == dbAuthor.FirstName))
                throw new AppException("The author has already been added");
        }

        _context.Authors.Add(author);
        _context.SaveChanges();

        return author;
    }

    public void Update(int id, UpdateAuthorRequest model)
    {
        var author = getById(id);

        var dbAuthors = _context.Authors;

        foreach (var dbAuthor in dbAuthors)
        {
            if ((author.LastName == dbAuthor.LastName) && (author.FirstName == dbAuthor.FirstName))
                throw new AppException("The author has already been added");
        }

        _mapper.Map(model, author);
        _context.Authors.Update(author);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var author = getById(id);
        _context.Authors.Remove(author);
        _context.SaveChanges();
    }

    //********************************
    //Helper Methods
    //********************************

    private Author getById(int id)
    {
        var author = _context.Authors.Find(id);
        if (author == null) throw new KeyNotFoundException("Author not found");
        return author;
    }

    private AuthorView createAuthorView(Author author)
    {
        return new AuthorView
        {
            AuthorId = author.Id,
            FirstName = author.FirstName,
            LastName = author.LastName,
            BookList = author.Books
        };
    }
}