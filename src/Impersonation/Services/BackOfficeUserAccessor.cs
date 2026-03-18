using System.Security.Claims;
using Impersonation.Interfaces.Accessors;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.Models.Membership;
using Umbraco.Cms.Core.Services;
using Umbraco.Extensions;

namespace Impersonation.Services;

public class BackofficeUserAccessor(
    IOptionsSnapshot<CookieAuthenticationOptions> cookieOptionsSnapshot,
    IHttpContextAccessor httpContextAccessor,
    IUserService userService
    ) : IBackofficeUserAccessor
{
    public ClaimsIdentity? BackofficeUser
    {
        get
        {
            var httpContext = httpContextAccessor.HttpContext;

            if (httpContext == null)
            {
                return new ClaimsIdentity();
            }


            var cookieOptions =
                cookieOptionsSnapshot.Get(Umbraco.Cms.Core.Constants.Security.BackOfficeAuthenticationType);
            var backOfficeCookie = httpContext.Request.Cookies[cookieOptions.Cookie.Name!];

            if (string.IsNullOrEmpty(backOfficeCookie))
            {
                return new ClaimsIdentity();
            }

            var unprotected = cookieOptions.TicketDataFormat.Unprotect(backOfficeCookie);
            var backOfficeIdentity = unprotected?.Principal.GetUmbracoIdentity();

            return backOfficeIdentity;
        }
    }

     public async Task<IUser?> GetUmbracoUser()
    {
        Guid? currentUserKey = BackofficeUser?.GetUserKey();

        if (currentUserKey is null)
        {
            var currentUserId = BackofficeUser?.GetUserId<int>();
            if (currentUserId.HasValue)
            {
                return userService.GetUserById(currentUserId.Value);
            }
        }
        else
        {
            return userService.GetAsync(currentUserKey.Value).GetAwaiter().GetResult();
        }

        return null;
    }
}
