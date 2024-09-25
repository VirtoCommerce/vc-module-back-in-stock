using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Extensions.Logging;
using VirtoCommerce.BackInStockModule.Core.BackgroundJobs;
using VirtoCommerce.BackInStockModule.Core.Models;
using VirtoCommerce.BackInStockModule.Core.Notifications;
using VirtoCommerce.BackInStockModule.Core.Services;
using VirtoCommerce.CatalogModule.Core.Model.Search;
using VirtoCommerce.CatalogModule.Core.Search;
using VirtoCommerce.CustomerModule.Core.Services;
using VirtoCommerce.NotificationsModule.Core.Extensions;
using VirtoCommerce.NotificationsModule.Core.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.StoreModule.Core.Model;
using VirtoCommerce.StoreModule.Core.Services;
using BackInStockSettings = VirtoCommerce.BackInStockModule.Core.ModuleConstants.Settings.General;

namespace VirtoCommerce.BackInStockModule.Data.BackgroundJobs;

public class BackInStockNotificationJobService(
    ISettingsManager settingsManager,
    INotificationSearchService notificationSearchService,
    INotificationSender notificationSender,
    IMemberResolver memberResolver,
    IProductSearchService productSearchService,
    IStoreService storeService,
    IBackInStockSubscriptionSearchService backInStockSubscriptionSearchService,
    ILogger<BackInStockNotificationJobService> logger)
    : IBackInStockNotificationJobService
{
    public void EnqueueProductBackInStockNotifications(IList<string> inStockProductsIds)
    {
        try
        {
            BackgroundJob.Enqueue(() =>
                EnqueueBatchOfEmailNotificationsForProductIds(inStockProductsIds));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Cannot enqueue back in stock notifications background job");
            throw;
        }
    }

    private async Task EnqueueBatchOfEmailNotificationsForProductIds(IList<string> inStockProductsIds)
    {
        foreach (var productId in inStockProductsIds)
        {
            try
            {
                var subscriptions = await FetchAllBackInStockSubscriptionsForProduct(productId);
                foreach (var subscription in subscriptions)
                {
                    BackgroundJob.Enqueue(() =>
                        SendBackInStockEmailNotificationAsync(subscription));
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Failed to enqueue batch of notification jobs for product id: {productId}");
                throw;
            }
        }
    }

    private async Task<IList<BackInStockSubscription>> FetchAllBackInStockSubscriptionsForProduct(string productId)
    {
        var allSubscriptions = new List<BackInStockSubscription>();
        int totalCount, skip = 0;
        do
        {
            var batchSize = await GetBatchSize();

            var result = await backInStockSubscriptionSearchService.SearchAsync(
                new BackInStockSubscriptionSearchCriteria
                {
                    ProductId = productId, IsActive = true, Skip = skip, Take = batchSize
                });

            allSubscriptions.AddRange(result.Results);
            totalCount = result.TotalCount;
            skip += batchSize;
        }
        while (allSubscriptions.Count < totalCount);

        return allSubscriptions;
    }

    private async Task SendBackInStockEmailNotificationAsync(BackInStockSubscription subscription)
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

        var store = (await storeService.GetAsync(new List<string> { subscription.StoreId }))
            .FirstOrDefault();
        if (store == null)
        {
            return null;
        }

        var product = (await productSearchService.SearchAsync(new ProductSearchCriteria
        {
            ObjectIds = new List<string> { subscription.ProductId }, Take = 1
        })).Results.FirstOrDefault();

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
