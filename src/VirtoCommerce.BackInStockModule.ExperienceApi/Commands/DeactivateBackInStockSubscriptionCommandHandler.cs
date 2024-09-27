using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VirtoCommerce.BackInStockModule.Core.Models;
using VirtoCommerce.BackInStockModule.Core.Services;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.BackInStockModule.ExperienceApi.Commands;

public class DeactivateBackInStockSubscriptionCommandHandler(
    IBackInStockSubscriptionService crudService,
    IBackInStockSubscriptionSearchService searchService)
    : IRequestHandler<DeactivateBackInStockSubscriptionCommand, BackInStockSubscription>
{
    public async Task<BackInStockSubscription> Handle(DeactivateBackInStockSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var searchCriteria = AbstractTypeFactory<BackInStockSubscriptionSearchCriteria>.TryCreateInstance();
        searchCriteria.ProductId = request.ProductId;
        searchCriteria.StoreId = request.StoreId;
        searchCriteria.UserId = request.UserId;
        searchCriteria.Take = 1;

        var subscription = (await searchService.SearchAsync(searchCriteria)).Results.FirstOrDefault();
        if (subscription != null)
        {
            subscription.IsActive = false;
            await crudService.SaveChangesAsync([subscription]);
        }

        return subscription;
    }
}
