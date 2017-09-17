using System;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Authorization.Security
{
    public class MinimumAgeRequirementHandler : AuthorizationHandler<MinimumAgeRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            MinimumAgeRequirement requirement)
        {
            var claim = context.User.FindFirst("birthdate");
            if (HasUserMinimumAge(claim, requirement))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }

        private bool HasUserMinimumAge(Claim claim, MinimumAgeRequirement requirement)
        {
            if (claim == null)
            {
                return false;
            }

            DateTime birthdate;
            if (DateTime.TryParse(claim.Value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal,
                out birthdate))
            {
                return GetCurrentAge(birthdate) >= requirement.Age;
            }
            return false;
        }

        private int GetCurrentAge(DateTime birthdate)
        {
            var now = DateTime.Now;
            var age = now.Year - birthdate.Year;

            if (now.Month < birthdate.Month || (now.Month == birthdate.Month && now.Day < birthdate.Day))
            {
                age--;
            }
            return age;
        }
    }
}