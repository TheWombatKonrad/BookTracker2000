using BookTrackersApi;
using BookTrackersApi.DatabaseContext;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services, builder.Environment);

var app = builder.Build();

// migrate any database changes on startup (includes initial db creation)
using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<SqliteDataContext>();
    dataContext.Database.Migrate();
}

startup.Configure(app);

app.Run("https://localhost:4000");
