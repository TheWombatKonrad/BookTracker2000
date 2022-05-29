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

        public void Configure(WebApplication app)
        {
            app.UseRouting();

            // global cors policy
            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            //app.UseIdentityServer();

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

        public void ConfigureServices(IServiceCollection services, IWebHostEnvironment env)
        {
            if (env.IsProduction())
                services.AddDbContext<SqlServerDataContext>();
            else
                services.AddDbContext<SqlServerDataContext, SqliteDataContext>();

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    policy =>
                    {
                        policy.WithOrigins("https://localhost:4000", "https://localhost:4001")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                    });
            });   

            services.AddControllers();
            services.AddControllers().AddJsonOptions(x =>
                            x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            //services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

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

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
            .AddIdentityServerAuthentication(options =>
            {
                options.Authority = "https://localhost:5001";
                options.ApiName = "app.api.OpenBookTrackers";
            });
        }
    }
}
