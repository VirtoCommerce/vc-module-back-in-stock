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

public class BackInStockNotificationJobService(
    ISettingsManager settingsManager,
    INotificationSearchService notificationSearchService,
    INotificationSender notificationSender,
    IMemberResolver memberResolver,
    IItemService itemService,
    IStoreService storeService,
    IBackInStockSubscriptionSearchService backInStockSubscriptionSearchService,
    ILogger<BackInStockNotificationJobService> logger)
    : IBackInStockNotificationJobService
{
    public void EnqueueProductBackInStockNotifications(IList<string> productIds)
    {
        try
        {
            BackgroundJob.Enqueue(() => EnqueueBatchOfEmailNotificationsForProductIds(productIds));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Cannot enqueue back in stock notifications background job");
            throw;
        }
    }

    public async Task EnqueueBatchOfEmailNotificationsForProductIds(IList<string> productIds)
    {
        foreach (var productId in productIds)
        {
            try
            {
                var searchCriteria = AbstractTypeFactory<BackInStockSubscriptionSearchCriteria>.TryCreateInstance();
                searchCriteria.ProductIds = [productId];
                searchCriteria.IsActive = true;
                searchCriteria.Sort = nameof(BackInStockSubscription.Triggered);
                searchCriteria.Take = await GetBatchSize();

                await foreach (var searchResult in backInStockSubscriptionSearchService.SearchBatchesAsync(searchCriteria))
                {
                    foreach (var backInStockSubscription in searchResult.Results)
                    {
                        await SendBackInStockEmailNotificationAsync(backInStockSubscription);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to enqueue batch of notification jobs for product id: {productId}", productId);
                throw;
            }
        }
    }

    public async Task SendBackInStockEmailNotificationAsync(BackInStockSubscription subscription)
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
            logger.LogError(ex, $"Failed to send {nameof(BackInStockEmailNotification)} notification");
            throw;
        }
    }

    private async Task<BackInStockEmailNotification> PrepareNotification(BackInStockSubscription subscription)
    {
        var notification = await notificationSearchService.GetNotificationAsync(
                nameof(BackInStockEmailNotification),
                new TenantIdentity(subscription.StoreId, nameof(Store)))
            as BackInStockEmailNotification;

        if (notification == null)
        {
            return null;
        }

        var customer = await memberResolver.ResolveMemberByIdAsync(subscription.UserId);
        if (customer == null || !customer.Emails.Any())
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

        notification.Item = product;
        notification.Customer = customer;
        notification.From = store.EmailWithName;
        notification.To = customer.Emails.First();
        notification.Type = nameof(BackInStockEmailNotification);

        return notification;
    }

    private Task<int> GetBatchSize()
    {
        return settingsManager.GetValueAsync<int>(BackInStockSettings.SubscriptionsJobBatchSize);
    }
}
