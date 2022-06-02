namespace IdentityServer
{
    public class Startup
    {
        public Startup(IConfigurationRoot configuration)
        {
            Configuration = configuration;
        }
        public IConfigurationRoot Configuration { get; }

        public void Configure(IApplicationBuilder app)
        {
            app.UseIdentityServer();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentityServer()
            .AddInMemoryApiResources(Config.ApiResources)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryClients(Config.Clients)
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddTestUsers(Config.TestUsers)
            .AddDeveloperSigningCredential();

        }
    }
}
