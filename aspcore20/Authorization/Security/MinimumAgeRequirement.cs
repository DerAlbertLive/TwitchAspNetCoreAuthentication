using Microsoft.AspNetCore.Authorization;

namespace Authorization.Security
{
    public class MinimumAgeRequirement : IAuthorizationRequirement
    {
        public MinimumAgeRequirement(int age)
        {
            Age = age;
        }

        public int Age { get; }
    }
}
