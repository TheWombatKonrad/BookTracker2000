using BookTrackersApi.Authorization;
using BookTrackersApi.DatabaseContext;
using BookTrackersApi.Helpers;
using BookTrackersApi.Services;
using IdentityServer4.AccessTokenValidation;
using System.Text.Json.Serialization;

namespace BookTrackersApi
{
    public class Startup
    {
        public Startup(IConfigurationRoot configuration)
        {
            Configuration = configuration;
        }
        public IConfigurationRoot Configuration { get; }

        //this runs at every request
        public void Configure(WebApplication app)
        {
            //the order of things is important so be careful when adding things

            app.UseRouting();

            // global cors policy
            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            // global error handler
            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseMiddleware<JwtMiddleware>();

            app.UseHttpsRedirection();

            app.MapControllers();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
        }

        //this runs when application starts
        public void ConfigureServices(IServiceCollection services, IWebHostEnvironment env)
        {
            //the order of things is important so be careful when adding things

            if (env.IsProduction())
                services.AddDbContext<SqlServerDataContext>();
            else
                services.AddDbContext<SqlServerDataContext, SqliteDataContext>();

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    policy =>
                    {
                        policy.WithOrigins("https://localhost:4000",//api
                            "https://localhost:4001",//webb
                            "https://localhost:5001")//identityserver
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                    });
            });   

            //so it doesn't cycle forever
            services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions
                            .ReferenceHandler = ReferenceHandler.IgnoreCycles);
;
            services.AddSwaggerGen();//acces via https://localhost:4000/swagger

            // configure automapper with all automapper profiles from this assembly
            services.AddAutoMapper(typeof(Program));

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            // configure DI for application services
            services.AddScoped<IJwtUtils, JwtUtils>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IReadingService, ReadingService>();

            //so httpcontext can be accessed in the services
            services.AddHttpContextAccessor();

            //config identityserver
            services.AddAuthentication("Bearer")
            .AddIdentityServerAuthentication("Bearer", options =>
            {
                options.Authority = "https://localhost:5001";
                options.ApiName = "app.api.OpenBookTrackers";
            });
        }
    }
}
