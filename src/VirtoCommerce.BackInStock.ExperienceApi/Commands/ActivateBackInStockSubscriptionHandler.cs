using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using VirtoCommerce.BackInStock.Core.Models;
using VirtoCommerce.BackInStock.Core.Services;

namespace VirtoCommerce.BackInStock.ExperienceApi.Commands;

public class ActivateBackInStockSubscriptionHandler(
    IBackInStockSubscriptionService backInStockSubscriptionService,
    IBackInStockSubscriptionSearchService backInStockSubscriptionSearchService,
    IMapper mapper)
    : IRequestHandler<ActivateBackInStockSubscriptionCommand, BackInStockSubscription>
{
    public async Task<BackInStockSubscription> Handle(ActivateBackInStockSubscriptionCommand request,
        CancellationToken cancellationToken)
    {
        var subscription =
            (await backInStockSubscriptionSearchService.SearchAsync(new BackInStockSubscriptionSearchCriteria()
            {
                ProductId = request.ProductId, StoreId = request.StoreId, UserId = request.UserId, Take = 1
            })).Results.FirstOrDefault();
        if (subscription != null)
        {
            subscription.IsActive = true;
            await backInStockSubscriptionService.SaveChangesAsync(new[] { subscription });
            return subscription;
        }

        var backInStockSubscription = mapper.Map<BackInStockSubscription>(request);
        await backInStockSubscriptionService.SaveChangesAsync(new[] { backInStockSubscription });
        return backInStockSubscription;
    }
}
