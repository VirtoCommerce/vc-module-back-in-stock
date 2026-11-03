using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using VirtoCommerce.BackInStock.Core.Models;
using VirtoCommerce.BackInStock.Core.Notifications;
using VirtoCommerce.CatalogModule.Core.Services;
using VirtoCommerce.CustomerModule.Core.Model;
using VirtoCommerce.CustomerModule.Core.Services;
using VirtoCommerce.NotificationsModule.Core.Extensions;
using VirtoCommerce.NotificationsModule.Core.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.StoreModule.Core.Model;
using VirtoCommerce.StoreModule.Core.Services;
using BackInStockSettings = VirtoCommerce.BackInStock.Core.ModuleConstants.Settings.General;

namespace VirtoCommerce.BackInStock.Data.BackgroundJobs;

/// <summary>
/// Builds the back in stock email for a subscription, or returns <c>null</c> when it must not be sent.
/// </summary>
public class BackInStockNotificationBuilder(
    INotificationSearchService notificationSearchService,
    IStoreService storeService,
    IItemService itemService,
    Func<UserManager<ApplicationUser>> userManagerFactory,
    IMemberService memberService)
{
    public virtual async Task<BackInStockEmailNotification> BuildAsync(BackInStockSubscription subscription)
    {
        var notification = await notificationSearchService.GetNotificationAsync<BackInStockEmailNotification>(new TenantIdentity(subscription.StoreId, nameof(Store)));
        if (notification == null)
        {
            return null;
        }

        var store = await GetEnabledStoreAsync(subscription.StoreId);
        if (store == null)
        {
            return null;
        }

        var product = await itemService.GetByIdAsync(subscription.ProductId);
        if (product == null)
        {
            return null;
        }

        var (member, recipientEmail) = await GetRecipientAsync(subscription.UserId);
        if (string.IsNullOrEmpty(recipientEmail))
        {
            return null;
        }

        notification.StoreUrl = store.Url?.TrimEnd('/');
        notification.Product = product;
        notification.Customer = member;
        notification.From = store.EmailWithName;
        notification.To = recipientEmail;

        return notification;
    }

    /// <summary>
    /// Returns the store when it exists and has back in stock notifications enabled; otherwise <c>null</c>.
    /// </summary>
    protected virtual async Task<Store> GetEnabledStoreAsync(string storeId)
    {
        var store = await storeService.GetByIdAsync(storeId);

        return store != null && store.Settings.GetValue<bool>(BackInStockSettings.Enable)
            ? store
            : null;
    }

    /// <summary>
    /// Returns the subscriber's contact and the email to send to: the contact's first email, or the user's email.
    /// </summary>
    protected virtual async Task<(Member Member, string Email)> GetRecipientAsync(string userId)
    {
        using var userManager = userManagerFactory();
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return (null, null);
        }

        var member = !string.IsNullOrEmpty(user.MemberId)
            ? await memberService.GetByIdAsync(user.MemberId)
            : null;

        return (member, member?.Emails?.FirstOrDefault() ?? user.Email);
    }
}
