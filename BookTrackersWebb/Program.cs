var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

//makes sure the html files are loaded
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();

//starts the webpage on the index
app.UsePathBase("/pages/index.html");

app.Run("https://localhost:4001");
