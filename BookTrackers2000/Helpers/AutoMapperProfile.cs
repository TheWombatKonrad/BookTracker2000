namespace BookTrackersApi.Helpers;

using AutoMapper;
using BookTrackersApi.Entities;
using BookTrackersApi.Models.Authors;
using BookTrackersApi.Models.Books;
using BookTrackersApi.Models.Readings;
using BookTrackersApi.Models.Users;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // User -> AuthenticateResponse
        CreateMap<User, AuthenticateResponse>();

        // RegisterRequest -> User
        CreateMap<RegisterUserRequest, User>();

        // UpdateRequest -> User
        CreateMap<UpdateUserRequest, User>()
            .ForAllMembers(x => x.Condition(
                (src, dest, prop) =>
                {
                    // ignore null & empty string properties
                    if (prop == null) return false;
                    if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                    return true;
                }
            ));

        // RegisterRequest -> Author
        CreateMap<RegisterAuthorRequest, Author>();

        // UpdateRequest -> Author
        CreateMap<UpdateAuthorRequest, Author>()
            .ForAllMembers(x => x.Condition(
                (src, dest, prop) =>
                {
                    // ignore null & empty string properties
                    if (prop == null) return false;
                    if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                    return true;
                }
            ));

        // RegisterRequest -> Book
        CreateMap<RegisterBookRequest, Book>();

        // RegisterRequest -> Reading
        CreateMap<RegisterReadingRequest, Reading>();

        // UpdateRequest -> Reading
        CreateMap<UpdateReadingRequest, Reading>()
            .ForAllMembers(x => x.Condition(
                (src, dest, prop) =>
                {
                    // ignore null & empty string properties
                    if (prop == null) return false;
                    if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                    return true;
                }
            ));


    }
}