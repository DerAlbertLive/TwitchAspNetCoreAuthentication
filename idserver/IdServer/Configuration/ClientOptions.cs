using System;
using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;

namespace IdServer.Configuration
{
    public class ClientOptions
    {
        public ClientOptions()
        {
        }

        /// <summary>
        /// Specifies if client is enabled (defaults to <c>true</c>)
        /// </summary>
        public bool Enabled { get; set; } = true;


        /// <summary>
        /// Unique ID of the client
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Client secrets - only relevant for flows that require a secret
        /// Must be hashed with SHA256 or SHA512, or a custom implementation
        /// </summary>
        public ICollection<SecretOptions> ClientSecrets { get; set; } = new List<SecretOptions>();

        /// <summary>
        /// If set to false, no client secret is needed to request tokens at the token endpoint 
        /// Not needed if only the IdToken is needed.
        /// </summary>
        public bool RequireClientSecret { get; set; } = false;

        /// <summary>
        /// Client display name (used for logging and consent screen)
        /// BTW: The NetFiles Version has no consent screen
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// URI to further information about client (used on consent screen)
        /// BTW: The NetFiles Version has no consent screen
        /// </summary>
        public string ClientUri { get; set; }

        /// <summary>
        /// URI to client logo (used on consent screen)
        /// BTW: The NetFiles Version has no consent screen
        /// </summary>
        public string LogoUri { get; set; }

        /// <summary>
        /// Specifies whether a consent screen is required
        /// BTW: The NetFiles Version has no consent screen
        /// </summary>
        public bool RequireConsent { get; set; } = false;

        /// <summary>
        /// Specifies whether user can choose to store consent decisions (defaults to <c>true</c>)
        /// BTW: The NetFiles Version has no consent screen
        /// </summary>
        public bool AllowRememberConsent { get; set; } = true;

        /// <summary>
        /// Specifies the allowed grant types (legal combinations of AuthorizationCode, Implicit, Hybrid, ResourceOwner, ClientCredentials). Defaults to Implicit.
        /// </summary>
        public ICollection<string> AllowedGrantTypes { get; set; } = new HashSet<string>(new[]{"implicit"});


        /// <summary>
        /// Controls whether access tokens are transmitted via the browser for this client (defaults to <c>false</c>).
        /// This can prevent accidental leakage of access tokens when multiple response types are allowed.
        /// </summary>
        /// <value>
        /// <c>true</c> if access tokens can be transmitted via the browser; otherwise, <c>false</c>.
        /// </value>
        public bool AllowAccessTokensViaBrowser { get; set; } = false;


        /// <summary>
        /// Specifies allowed URIs to return tokens or authorization codes to
        /// </summary>
        public ICollection<string> RedirectUris { get; set; } = new HashSet<string>();

        /// <summary>
        /// Specifies allowed URIs to redirect to after logout
        /// </summary>
        public ICollection<string> PostLogoutRedirectUris { get; set; } = new HashSet<string>();

        /// <summary>
        /// Specifies logout URI at client for HTTP front-channel based logout.
        /// </summary>
        public string FrontChannelLogoutUri { get; set; }

        /// <summary>
        /// Specifies is the user's session id should be sent to the FrontChannelLogoutUri. Defaults to <c>true</c>.
        /// </summary>
        public bool FrontChannelLogoutSessionRequired { get; set; } = true;

        /// <summary>
        /// Specifies logout URI at client for HTTP back-channel based logout.
        /// </summary>
        public string BackChannelLogoutUri { get; set; }

        /// <summary>
        /// Specifies is the user's session id should be sent to the BackChannelLogoutUri. Defaults to <c>true</c>.
        /// </summary>
        public bool BackChannelLogoutSessionRequired { get; set; } = true;

        /// <summary>
        /// Specifies the api scopes that the client is allowed to request. If empty, the client can't access any scope
        /// </summary>
        public ICollection<string> AllowedScopes { get; set; } = new HashSet<string>(new[] {"email", "openid"});

        /// <summary>
        /// When requesting both an id token and access token, should the user claims always be added to the id token instead of requring the client to use the userinfo endpoint.
        /// Defaults to <c>false</c>.
        /// </summary>
        public bool AlwaysIncludeUserClaimsInIdToken { get; set; } = false;

        /// <summary>
        /// Lifetime of identity token in seconds (defaults to 300 seconds / 5 minutes)
        /// </summary>
        public int IdentityTokenLifetime { get; set; } = 300;

        /// <summary>
        /// Lifetime of access token in seconds (defaults to 3600 seconds / 1 hour)
        /// </summary>
        public int AccessTokenLifetime { get; set; } = 3600;

        /// <summary>
        /// Lifetime of authorization code in seconds (defaults to 300 seconds / 5 minutes)
        /// </summary>
        public int AuthorizationCodeLifetime { get; set; } = 300;

        /// <summary>
        /// Gets or sets a value indicating whether JWT access tokens should include an identifier. Defaults to <c>false</c>.
        /// </summary>
        /// <value>
        /// <c>true</c> to add an id; otherwise, <c>false</c>.
        /// </value>
        public bool IncludeJwtId { get; set; } = false;

        /// <summary>
        /// Allows settings claims for the client (will be included in the access token).
        /// </summary>
        /// <value>
        /// The claims.
        /// </value>
        public ICollection<Claim> Claims { get; set; } = new HashSet<Claim>(new ClaimComparer());

        /// <summary>
        /// Gets or sets a value indicating whether client claims should be always included in the access tokens - or only for client credentials flow.
        /// Defaults to <c>false</c>
        /// </summary>
        /// <value>
        /// <c>true</c> if claims should always be sent; otherwise, <c>false</c>.
        /// </value>
        public bool AlwaysSendClientClaims { get; set; } = false;


        /// <summary>
        /// Gets or sets a value indicating whether all client claims should be prefixed. Defaults to <c>true</c>.
        /// </summary>
        /// <value>
        /// <c>true</c> if client claims should be prefixed; otherwise, <c>false</c>.
        /// </value>
        public bool PrefixClientClaims { get; set; } = true;

        /// <summary>
        /// Gets or sets the allowed CORS origins for JavaScript clients.
        /// </summary>
        /// <value>
        /// The allowed CORS origins.
        /// </value>
        public ICollection<string> AllowedCorsOrigins { get; set; } = new HashSet<string>();
    }

    public class SecretOptions
    {
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the expiration.
        /// </summary>
        /// <value>
        /// The expiration.
        /// </value>
        public DateTime? Expiration { get; set; }
    }
}