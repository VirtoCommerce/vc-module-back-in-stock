using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VirtoCommerce.BackInStock.Core.BackgroundJobs;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Jobs;

namespace VirtoCommerce.BackInStock.Data.BackgroundJobs;

public class BackInStockNotificationJob(
    BackInStockNotificationJobHandler jobHandler,
    ILogger<BackInStockNotificationJob> logger)
    : IBackInStockNotificationJob
{
    // The enqueue is not awaited because the interface method is synchronous.
    public void Enqueue(IList<string> productIds)
    {
        _ = EnqueueAsync(productIds);
    }

    /// <summary>
    /// Kept for background jobs enqueued by an earlier version, which reference this method by name.
    /// New work goes through <see cref="BackInStockNotificationJobHandler"/>; remove this once no such job
    /// can still be pending.
    /// </summary>
    [Obsolete("Enqueued indirectly by legacy Hangfire jobs only; new work uses BackInStockNotificationJobHandler.", DiagnosticId = "VC0015", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public Task Run(IList<string> productIds)
    {
        var payload = AbstractTypeFactory<BackInStockNotificationJobPayload>.TryCreateInstance();
        payload.ProductIds = productIds;

        return jobHandler.Execute(payload, context: null);
    }

    private async Task EnqueueAsync(IList<string> productIds)
    {
        try
        {
            var payload = AbstractTypeFactory<BackInStockNotificationJobPayload>.TryCreateInstance();
            payload.ProductIds = productIds;

            // The static facade, not an injected IBackgroundJob: this service is a singleton, so it must not capture
            // a Scoped dependency.
            await BackgroundJob.Enqueue<BackInStockNotificationJobHandler>(payload);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to enqueue back in stock notifications background job");
        }
    }
}
