using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VirtoCommerce.BackInStockModule.Core.Models;
using VirtoCommerce.BackInStockModule.Core.Services;

namespace VirtoCommerce.BackInStockModule.ExperienceApi.Commands;

public class DeactivateBackInStockSubscriptionHandler(
    IBackInStockSubscriptionService backInStockSubscriptionService,
    IBackInStockSubscriptionSearchService backInStockSubscriptionSearchService)
    : IRequestHandler<DeactivateBackInStockSubscriptionCommand, BackInStockSubscription>
{
    public async Task<BackInStockSubscription> Handle(DeactivateBackInStockSubscriptionCommand request,
        CancellationToken cancellationToken)
    {
        var subscription =
            (await backInStockSubscriptionSearchService.SearchAsync(new BackInStockSubscriptionSearchCriteria()
            {
                ProductId = request.ProductId, StoreId = request.StoreId, UserId = request.UserId, Take = 1
            })).Results.FirstOrDefault();
        if (subscription != null)
        {
            subscription.IsActive = false;
            await backInStockSubscriptionService.SaveChangesAsync(new[] { subscription });
            return subscription;
        }

        throw new InvalidOperationException($"Subscription not found");
    }
}
