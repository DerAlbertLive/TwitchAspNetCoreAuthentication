using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;

namespace Cookie.Services
{
    public interface IUserClaimsService
    {
        IEnumerable<Claim> GetClaimsForUser(string name);
    }
}