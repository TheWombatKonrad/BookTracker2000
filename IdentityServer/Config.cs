using IdentityServer4.Models;

namespace IdentityServer
{
    public class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

        public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("app.api.OpenBookTrackers.read"),
            new ApiScope("app.api.OpenBookTrackers.write")
        };

        public static IEnumerable<ApiResource> ApiResources =>
        new List<ApiResource>
        {
            new ApiResource
            {
                Name = "app.api.OpenBookTrackers",
                DisplayName = "OpenBookTrackers Api",
                ApiSecrets = { new Secret("supersecret".Sha256()) },
                Scopes = new List<string> //should be scope according to guide?
                {
                    "app.api.OpenBookTrackers.read",
                    "app.api.OpenBookTrackers.write"
                },
            }
        };

        public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            new Client
            {
                ClientId = "SomeClient",
                ClientName = "SomeClient I Used To Know",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = new List<Secret> { new Secret("secret".Sha256()) },
                AllowedScopes = new List<string>
                {
                    "app.api.OpenBookTrackers.read",
                    "app.api.OpenBookTrackers.write"
                }
            }
        };
    }
}
