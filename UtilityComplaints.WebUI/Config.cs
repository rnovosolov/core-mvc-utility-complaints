using IdentityServer4.Models;

namespace UtilityComplaints.WebUI
{
    public static class Config
    {



        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "WebUI",
                    ClientSecrets = { new Secret("abc") },//encode

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    //redirect uri
                    //logout redirect uri

                    //offline access
                    //AllowedScopes = { "scope" }


                },
            };

    };
}
