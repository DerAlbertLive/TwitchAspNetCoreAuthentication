using System.Collections.Generic;
using System.Security.Claims;

namespace DataProtection.Services
{
    public interface IUserClaimsService
    {
        IEnumerable<Claim> GetClaimsForUser(string name);
    }
}