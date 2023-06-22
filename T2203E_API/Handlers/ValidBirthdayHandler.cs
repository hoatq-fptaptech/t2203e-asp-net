using System;
using Microsoft.AspNetCore.Authorization;
using T2203E_API.Requirements;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;

namespace T2203E_API.Handlers
{
    public class ValidBirthdayHandler : AuthorizationHandler<YearOldRequirement>
    {
       
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, YearOldRequirement requirement)
        {
            if (IsValidBirthday(context.User,requirement))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
            return Task.CompletedTask;
        }

        private bool IsValidBirthday(ClaimsPrincipal user,YearOldRequirement requirement)
        {
            if (user == null) return false;
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            var dataContext = new T2203E_API.Entities.T2203eApiContext();
            var userData = dataContext.Users.Find(Convert.ToInt32(userId));
            if (userData == null || userData.Birthday == null) return false;
            var birthday = DateTime.Parse(userData.Birthday.ToString());
            var diffYear = DateTime.Today.Year - birthday.Year;
            if (diffYear >= requirement.MinYear && diffYear <= requirement.MaxYear) return true;
            
          
            return false;
        }
    }
}

