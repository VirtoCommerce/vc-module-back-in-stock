using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VirtoCommerce.BackInStock.Core.Extensions;
using VirtoCommerce.BackInStock.Core.Models;
using VirtoCommerce.BackInStock.Core.Notifications;
using VirtoCommerce.BackInStock.Core.Services;
using VirtoCommerce.NotificationsModule.Core.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Jobs;
using VirtoCommerce.Platform.Core.Settings;
using BackInStockSettings = VirtoCommerce.BackInStock.Core.ModuleConstants.Settings.General;

namespace VirtoCommerce.BackInStock.Data.BackgroundJobs;

/// <summary>
/// Sends back in stock notifications to the active subscribers of the given products and deactivates the subscriptions.
/// </summary>
public class BackInStockNotificationJobHandler(
    ISettingsManager settingsManager,
    BackInStockNotificationBuilder notificationBuilder,
    INotificationSender notificationSender,
    IBackInStockSubscriptionService subscriptionService,
    IBackInStockSubscriptionSearchService subscriptionSearchService,
    ILogger<BackInStockNotificationJobHandler> logger)
    : IBackgroundJobHandler<BackInStockNotificationJobPayload>
{
    public virtual async Task Execute(BackInStockNotificationJobPayload payload, IJobExecutionContext context, CancellationToken cancellationToken = default)
    {
        foreach (var productId in payload.ProductIds)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                await SendNotifications(productId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to send notifications for product {ProductId}", productId);
            }
        }
    }


    private async Task SendNotifications(string productId)
    {
        var searchCriteria = AbstractTypeFactory<BackInStockSubscriptionSearchCriteria>.TryCreateInstance();
        searchCriteria.ProductIds = [productId];
        searchCriteria.IsActive = true;
        searchCriteria.Take = await settingsManager.GetValueAsync<int>(BackInStockSettings.BatchSize);

        await subscriptionSearchService.SearchWhileResultIsNotEmpty(searchCriteria, async searchResult =>
        {
            foreach (var subscription in searchResult.Results)
            {
                await SendNotification(subscription);

                subscription.IsActive = false;
                await subscriptionService.SaveChangesAsync([subscription]);
            }
        });
    }

    private async Task SendNotification(BackInStockSubscription subscription)
    {
        try
        {
            var notification = await notificationBuilder.BuildAsync(subscription);
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
}
