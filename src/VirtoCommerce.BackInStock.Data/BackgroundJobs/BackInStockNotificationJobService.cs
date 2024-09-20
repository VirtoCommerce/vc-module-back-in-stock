using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using VirtoCommerce.BackInStock.Core.BackgroundJobs;
using VirtoCommerce.BackInStock.Core.Models;
using VirtoCommerce.BackInStock.Core.Notifications;
using VirtoCommerce.BackInStock.Core.Services;
using VirtoCommerce.CatalogModule.Core.Model.Search;
using VirtoCommerce.CatalogModule.Core.Search;
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
    IProductSearchService productSearchService,
    IStoreService storeService,
    IBackInStockSubscriptionSearchService backInStockSubscriptionSearchService)
    : IBackInStockNotificationJobService
{
    public void EnqueueProductBackInStockNotifications(IList<string> inStockProductsIds)
    {
        BackgroundJob.Enqueue<BackInStockNotificationJobService>(x =>
            x.TrackNewRecipientsRecurringJob(inStockProductsIds, JobCancellationToken.Null));
    }

    [DisableConcurrentExecution(10)]
    private async Task TrackNewRecipientsRecurringJob(IList<string> inStockProductsIds,
        IJobCancellationToken cancellationToken)
    {
        foreach (var productId in inStockProductsIds)
        {
            var backInStockSubscriptionSearchResult =
                await backInStockSubscriptionSearchService.SearchAsync(
                    new BackInStockSubscriptionSearchCriteria()
                    {
                        ProductId = productId, IsActive = true, Skip = 0, Take = await GetBatchSize(),
                    });

            foreach (var subscription in backInStockSubscriptionSearchResult.Results)
            {
                BackgroundJob.Enqueue<BackInStockNotificationJobService>(x =>
                    x.SendBackInStockEmailNotificationAsync(subscription));
            }
        }
    }

    private async Task SendBackInStockEmailNotificationAsync(BackInStockSubscription subscription)
    {
        var notification = await notificationSearchService.GetNotificationAsync(
                nameof(BackInStockEmailNotification),
                new TenantIdentity(subscription.StoreId, nameof(Store)))
            as BackInStockEmailNotification;

        if (notification != null)
        {
            var customer = await memberResolver.ResolveMemberByIdAsync(subscription.UserId);
            var store = (await storeService.GetAsync(new List<string>() { subscription.StoreId }))
                .FirstOrDefault();
            var product = await productSearchService.SearchAsync(new ProductSearchCriteria()
            {
                ObjectIds = new List<string>() { subscription.ProductId }
            });
            //notification.Item =
            notification.Customer = customer;
            //notification.RequestId = parameters.RequestId;
            //notification.LanguageCode = product.Results.FirstOrDefault().
            notification.From = store.EmailWithName;
            notification.To = customer.Emails.FirstOrDefault();

            await notificationSender.ScheduleSendNotificationAsync(notification);
        }
    }

    private Task<int> GetBatchSize()
    {
        return settingsManager.GetValueAsync<int>(BackInStockSettings.SubscriptionsJobBatchSize);
    }
}
