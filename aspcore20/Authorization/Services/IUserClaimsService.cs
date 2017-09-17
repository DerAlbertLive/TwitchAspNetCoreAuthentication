using System.Collections.Generic;
using System.Security.Claims;

namespace Authorization.Services
{
    public interface IUserClaimsService
    {
        IEnumerable<Claim> GetClaimsForUser(string name);
    }
}