using System.Collections.Generic;
using System.Linq;
using IdentityServer4.Models;
using IdServer.Configuration;
using Microsoft.Extensions.Configuration;

namespace IdServer.Loader
{
    public class ResourceLoader
    {
        public static IEnumerable<ApiResource> LoadApiResources(IConfiguration configuration)
        {
            var resources = configuration.GetSection("Scopes:Api").Get<ApiScopeOption[]>();

            if (resources == null)
            {
                return new ApiResource[0];
            }
            
            // Vereinfachte Api Scope Resource erstellung, nur ein Scope pro Api Resource hier notwendig, wenn überhaupt ;)
            var customResources = from r in resources
                select new ApiResource(r.Name, r.DisplayName, r.UserClaims)
                {
                    Enabled = r.Enabled,
                    Description = r.Description,
                    Scopes = new HashSet<Scope>(new[]
                    {
                        new Scope(r.Name,r.DisplayName,r.UserClaims)
                        {
                            Emphasize = r.Emphasize,
                            Required = r.Required,
                            ShowInDiscoveryDocument = r.ShowInDiscoveryDocument,
                            Description = r.Description
                        } 
                    })
                };

            return customResources;
        }

        public static IEnumerable<IdentityResource> LoadIdentityResources(IConfiguration configuration)
        {
            var defaultResources =  new IdentityResource[]
            {
                // some standard scopes from the OIDC spec
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };

            var resources = configuration.GetSection("Scopes:Identity").Get<IdentityResourceOption[]>();

            if (resources == null)
            {
                return defaultResources;
            }
            
            var customResources = from r in resources
                select new IdentityResource(r.Name, r.DisplayName, r.UserClaims)
                {
                    Enabled = r.Enabled,
                    Description = r.Description,
                    Emphasize = r.Emphasize,
                    Required = r.Required,
                    ShowInDiscoveryDocument = r.ShowInDiscoveryDocument
                };
            
            return defaultResources.Concat(customResources);
        }
    }
}