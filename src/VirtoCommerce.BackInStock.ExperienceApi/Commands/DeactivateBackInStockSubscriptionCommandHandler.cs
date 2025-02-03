using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VirtoCommerce.BackInStock.Core.Models;
using VirtoCommerce.BackInStock.Core.Services;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.BackInStock.ExperienceApi.Commands;

public class DeactivateBackInStockSubscriptionCommandHandler(
    IBackInStockSubscriptionService crudService,
    IBackInStockSubscriptionSearchService searchService)
    : IRequestHandler<DeactivateBackInStockSubscriptionCommand, BackInStockSubscription>
{
    public async Task<BackInStockSubscription> Handle(DeactivateBackInStockSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var searchCriteria = AbstractTypeFactory<BackInStockSubscriptionSearchCriteria>.TryCreateInstance();
        searchCriteria.StoreId = request.StoreId;
        searchCriteria.ProductIds = [request.ProductId];
        searchCriteria.UserId = request.UserId;
        searchCriteria.Take = 1;

        var searchResult = await searchService.SearchAsync(searchCriteria);

        var subscription = searchResult.Results.FirstOrDefault();
        if (subscription == null)
        {
            return null;
        }

        subscription.IsActive = false;
        await crudService.SaveChangesAsync([subscription]);

        return subscription;
    }
}
