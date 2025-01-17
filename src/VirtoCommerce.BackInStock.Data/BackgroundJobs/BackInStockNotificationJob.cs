using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Extensions.Logging;
using VirtoCommerce.BackInStock.Core.BackgroundJobs;
using VirtoCommerce.BackInStock.Core.Models;
using VirtoCommerce.BackInStock.Core.Notifications;
using VirtoCommerce.BackInStock.Core.Services;
using VirtoCommerce.CatalogModule.Core.Services;
using VirtoCommerce.CustomerModule.Core.Services;
using VirtoCommerce.NotificationsModule.Core.Extensions;
using VirtoCommerce.NotificationsModule.Core.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.StoreModule.Core.Model;
using VirtoCommerce.StoreModule.Core.Services;
using BackInStockSettings = VirtoCommerce.BackInStock.Core.ModuleConstants.Settings.General;

namespace VirtoCommerce.BackInStock.Data.BackgroundJobs;

public class BackInStockNotificationJob(
    ISettingsManager settingsManager,
    INotificationSearchService notificationSearchService,
    INotificationSender notificationSender,
    IMemberResolver memberResolver,
    IItemService itemService,
    IStoreService storeService,
    IBackInStockSubscriptionSearchService subscriptionSearchService,
    ILogger<BackInStockNotificationJob> logger)
    : IBackInStockNotificationJob
{
    public void EnqueueProductBackInStockNotifications(IList<string> productIds)
    {
        try
        {
            BackgroundJob.Enqueue(() => SendEmailNotificationsForProductIds(productIds));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to enqueue back in stock notifications background job");
            throw;
        }
    }

    public async Task SendEmailNotificationsForProductIds(IList<string> productIds)
    {
        foreach (var productId in productIds)
        {
            try
            {
                var searchCriteria = AbstractTypeFactory<BackInStockSubscriptionSearchCriteria>.TryCreateInstance();
                searchCriteria.ProductIds = [productId];
                searchCriteria.IsActive = true;
                searchCriteria.Take = await settingsManager.GetValueAsync<int>(BackInStockSettings.JobBatchSize);

                searchCriteria.Sort = new[]
                    {
                        new SortInfo { SortColumn = nameof(BackInStockSubscription.CreatedDate) },
                        new SortInfo { SortColumn = nameof(BackInStockSubscription.Id) },
                    }
                    .ToString();

                await foreach (var searchResult in subscriptionSearchService.SearchBatchesAsync(searchCriteria))
                {
                    foreach (var subscription in searchResult.Results)
                    {
                        await ScheduleNotificationAsync(subscription);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to send notifications for product {ProductId}", productId);
                throw;
            }
        }
    }

    private async Task ScheduleNotificationAsync(BackInStockSubscription subscription)
    {
        try
        {
            var notification = await PrepareNotification(subscription);
            if (notification != null)
            {
                await notificationSender.ScheduleSendNotificationAsync(notification);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send notification {NotificationType}", nameof(BackInStockEmailNotification));
            throw;
        }
    }

    private async Task<BackInStockEmailNotification> PrepareNotification(BackInStockSubscription subscription)
    {
        var notification = await notificationSearchService.GetNotificationAsync<BackInStockEmailNotification>(new TenantIdentity(subscription.StoreId, nameof(Store)));
        if (notification == null)
        {
            return null;
        }

        var customer = await memberResolver.ResolveMemberByIdAsync(subscription.UserId);
        if (customer == null || customer.Emails.Count == 0)
        {
            return null;
        }

        var store = await storeService.GetByIdAsync(subscription.StoreId);
        if (store == null)
        {
            return null;
        }

        var product = await itemService.GetByIdAsync(subscription.ProductId);
        if (product == null)
        {
            return null;
        }

        notification.Product = product;
        notification.Customer = customer;
        notification.From = store.EmailWithName;
        notification.To = customer.Emails.First();

        return notification;
    }
}
