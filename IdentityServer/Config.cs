using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Security.Claims;

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

        //adds new scopes
        public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("app.api.OpenBookTrackers.read"),
            new ApiScope("app.api.OpenBookTrackers.write")
        };

        //adds the api resource 
        public static IEnumerable<ApiResource> ApiResources =>
        new List<ApiResource>
        {
            new ApiResource
            {
                Name = "app.api.OpenBookTrackers",
                DisplayName = "OpenBookTrackers Api",
                ApiSecrets = { new Secret("supersecret".Sha256()) },
                Scopes = new List<string> 
                {
                    "app.api.OpenBookTrackers.read",
                    "app.api.OpenBookTrackers.write"
                },
            }
        };

        //ex client and what they can do
        public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            new Client
            {
                ClientId = "SomeClient",
                ClientName = "SomeClient I Used To Know",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = new List<Secret> { new Secret("123hund".Sha256()) },
                AllowedScopes = new List<string>
                {
                    "app.api.OpenBookTrackers.read",
                    "app.api.OpenBookTrackers.write"
                }
            }
        };

        //testusers 
        public static List<TestUser> TestUsers =>

                new List<TestUser>
                {
                    new TestUser
                    {
                        Username = "IdentityServerPerson",
                        Password =  "1234",
                        SubjectId = "1",
                        Claims =
                        {

                            new Claim(JwtClaimTypes.Name," Magnus")

                        }

                    }
                };
    }
}
