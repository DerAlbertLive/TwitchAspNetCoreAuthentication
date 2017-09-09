using System;
using System.Collections.Generic;
using System.Linq;
using IdentityServer4.Models;
using IdServer.Configuration;
using Microsoft.Extensions.Configuration;

namespace IdServer.Loader
{
    public class ClientLoader 
    {

        public static IEnumerable<Client> LoadClient(IConfiguration configuration)
        {
            var clients = configuration.GetSection("Clients").Get<ClientOptions[]>();

            if (clients == null || clients.Length == 0)
            {
                throw new InvalidOperationException("Missing ClinetConfiguration in appsettings.json");
            }

            return from c in clients select Map(c);
        }
        
        private static Client Map(ClientOptions options)
        {
            var client = new Client()
            {
                Enabled = options.Enabled,
                ClientId = options.ClientId,
                RequireClientSecret = options.RequireClientSecret,
                ClientName = options.ClientName,
                ClientUri = options.ClientUri,
                LogoUri = options.LogoUri,
                RequireConsent = options.RequireConsent,
                AllowRememberConsent = options.AllowRememberConsent,
                AllowAccessTokensViaBrowser = options.AllowAccessTokensViaBrowser,
                FrontChannelLogoutSessionRequired = options.FrontChannelLogoutSessionRequired,
                FrontChannelLogoutUri = options.FrontChannelLogoutUri,
                BackChannelLogoutSessionRequired = options.BackChannelLogoutSessionRequired,
                BackChannelLogoutUri = options.BackChannelLogoutUri,
                AlwaysIncludeUserClaimsInIdToken = options.AlwaysIncludeUserClaimsInIdToken,
                IdentityTokenLifetime = options.IdentityTokenLifetime,
                AccessTokenLifetime = options.AccessTokenLifetime,
                AuthorizationCodeLifetime = options.AuthorizationCodeLifetime,
                IncludeJwtId = options.IncludeJwtId,
                AlwaysSendClientClaims = options.AlwaysSendClientClaims,
                PrefixClientClaims = options.PrefixClientClaims
            };

            if (options.ClientSecrets.Any())
            {
                client.ClientSecrets = Map(options.ClientSecrets);
            }

            if (options.AllowedGrantTypes.Any())
            {
                client.AllowedGrantTypes = options.AllowedGrantTypes;
            }


            if (options.RedirectUris.Any())
            {
                client.RedirectUris = options.RedirectUris;
            }

            if (options.PostLogoutRedirectUris.Any())
            {
                client.PostLogoutRedirectUris = options.PostLogoutRedirectUris;
            }

            if (options.AllowedScopes.Any())
            {
                client.AllowedScopes = options.AllowedScopes;
            }
            
            if (options.Claims.Any())
            {
                client.Claims = options.Claims;
            }
            
            if (options.AllowedCorsOrigins.Any())
            {
                client.AllowedCorsOrigins = options.AllowedCorsOrigins;
            }
            return client;
        }

        private static ICollection<Secret> Map(IEnumerable<SecretOptions> optionsClientSecrets)
        {
            return new HashSet<Secret>(from so in optionsClientSecrets
                                       select new Secret(so.Value, so.Description, so.Expiration));
        }

    }
}