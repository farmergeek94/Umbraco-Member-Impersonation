using System.Security.Claims;
using Umbraco.Cms.Core.Models.Membership;

namespace Impersonation.Interfaces.Accessors;

public interface IBackofficeUserAccessor
{
    public ClaimsIdentity? BackofficeUser { get; }
    public Task<IUser?> GetUmbracoUser();
}
