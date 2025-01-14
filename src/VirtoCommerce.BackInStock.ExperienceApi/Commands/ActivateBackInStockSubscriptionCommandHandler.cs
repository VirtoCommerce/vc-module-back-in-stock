using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using VirtoCommerce.BackInStock.Core.Models;
using VirtoCommerce.BackInStock.Core.Services;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.BackInStock.ExperienceApi.Commands;

public class ActivateBackInStockSubscriptionCommandHandler(
    IBackInStockSubscriptionService crudService,
    IBackInStockSubscriptionSearchService searchService,
    IMapper mapper)
    : IRequestHandler<ActivateBackInStockSubscriptionCommand, BackInStockSubscription>
{
    public async Task<BackInStockSubscription> Handle(ActivateBackInStockSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var searchCriteria = AbstractTypeFactory<BackInStockSubscriptionSearchCriteria>.TryCreateInstance();
        searchCriteria.ProductIds = [request.ProductId];
        searchCriteria.StoreId = request.StoreId;
        searchCriteria.UserId = request.UserId;
        searchCriteria.Take = 1;

        var subscription = (await searchService.SearchAsync(searchCriteria)).Results.FirstOrDefault();
        if (subscription == null)
        {
            subscription = mapper.Map<BackInStockSubscription>(request);
        }

        subscription.IsActive = true;
        await crudService.SaveChangesAsync([subscription]);
        return subscription;
    }
}
