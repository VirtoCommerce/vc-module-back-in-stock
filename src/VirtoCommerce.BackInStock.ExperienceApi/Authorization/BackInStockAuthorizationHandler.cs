using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using VirtoCommerce.BackInStock.ExperienceApi.Commands;
using VirtoCommerce.BackInStock.ExperienceApi.Queries;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.StoreModule.Core.Services;

namespace VirtoCommerce.BackInStock.ExperienceApi.Authorization;

public class BackInStockAuthorizationRequirement : IAuthorizationRequirement;

public class BackInStockAuthorizationHandler(Func<UserManager<ApplicationUser>> userManagerFactory, IStoreService storeService)
    : AuthorizationHandler<BackInStockAuthorizationRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, BackInStockAuthorizationRequirement requirement)
    {
        var authorized = false;

        // Anonymous users are not allowed to perform any requests
        var currentUser = await GetCurrentUser(context);
        if (currentUser != null)
        {
            switch (context.Resource)
            {
                case BackInStockSubscriptionsQuery query:
                    authorized = await CanAccessStore(query.StoreId, currentUser);
                    break;
                case ActivateBackInStockSubscriptionCommand command:
                    authorized = await CanAccessStore(command.StoreId, currentUser);
                    break;
                case DeactivateBackInStockSubscriptionCommand:
                    authorized = true;
                    break;
            }
        }

        if (authorized)
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
    }



    private async Task<ApplicationUser> GetCurrentUser(AuthorizationHandlerContext context)
    {
        var userId = context.User.GetUserId();
        if (userId == null)
        {
            return null;
        }

        var userManager = userManagerFactory();
        var user = await userManager.FindByIdAsync(userId);

        return user;
    }

    private async Task<bool> CanAccessStore(string storeId, ApplicationUser user)
    {
        var store = await storeService.GetNoCloneAsync(storeId);
        if (store == null)
        {
            return false;
        }

        return user.StoreId.EqualsIgnoreCase(store.Id) || store.TrustedGroups.Any(user.StoreId.EqualsIgnoreCase);
    }
}
