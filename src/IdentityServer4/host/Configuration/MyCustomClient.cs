using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerHost.Configuration
{
    public static class MyCustomClient
    {
        public static IEnumerable<Client> Get()
        {
            return new List<Client> { new Client { 
            
                 ClientId ="myCustomClientId",
                 ClientName="myCustomClient",
                 AllowedGrantTypes=  GrantTypes.ClientCredentials,
                 ClientSecrets={ new Secret ("9A3207FB-7FFF-40F0-8ABF-214F1141E2BB".Sha256())},
                 AllowedScopes={ "api1" }
            } };
        }
    }
}
