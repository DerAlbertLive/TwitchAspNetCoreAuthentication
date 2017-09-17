using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Authorization.Services
{
    class UserClaimsService : IUserClaimsService
    {
        public IEnumerable<Claim> GetClaimsForUser(string name)
        {
            switch (name.ToLowerInvariant())
            {
                case "alice":
                    return GetAliceClaims();
                case "bob":
                    return GetBobClaims();
            }
            return  new Claim[0];
        }

        private IEnumerable<Claim> GetBobClaims()
        {
            return new[]
            {
                new Claim("birthdate", "12/01/1971"), 
                new Claim("mandant_id", "2"),
                new Claim("invoice", "read"),
                new Claim("name", "Bob Marley"),
                new Claim(JwtRegisteredClaimNames.GivenName, "Bob"),
                new Claim(JwtRegisteredClaimNames.FamilyName, "Marley"),
                new Claim(JwtRegisteredClaimNames.Email, "bob@example.com"),
                new Claim("email_verified", "false", ClaimValueTypes.Boolean),
            };
        }

        private IEnumerable<Claim> GetAliceClaims()
        {
            return  new[]
            {
                new Claim("birthdate", "12/01/2005"),
                new Claim("name", "Alice Smith"),
                new Claim("invoice", "write"),
                new Claim(JwtRegisteredClaimNames.GivenName, "Alice"),
                new Claim(JwtRegisteredClaimNames.FamilyName, "Smith"),
                new Claim(JwtRegisteredClaimNames.Email, "AliceSmith@email.com"),
                new Claim("email_verified", "true", ClaimValueTypes.Boolean),
            };
        }
    }
}