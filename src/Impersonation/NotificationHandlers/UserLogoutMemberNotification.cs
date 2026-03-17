using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Web.Common.Security;

namespace Impersonation.NotificationHandlers;

public class UserLogoutMemberNotification : INotificationAsyncHandler<UserLogoutSuccessNotification>
{
    private readonly IMemberSignInManager _memberSignInManager;

    public UserLogoutMemberNotification(IMemberSignInManager memberSignInManager) =>
        _memberSignInManager = memberSignInManager;

    public async Task HandleAsync(UserLogoutSuccessNotification notification, CancellationToken cancellationToken)
    {
        await _memberSignInManager.SignOutAsync();
    }
}
