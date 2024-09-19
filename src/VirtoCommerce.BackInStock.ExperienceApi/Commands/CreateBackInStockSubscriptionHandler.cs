using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using VirtoCommerce.BackInStock.Core.Models;
using VirtoCommerce.BackInStock.Core.Services;

namespace VirtoCommerce.BackInStock.ExperienceApi.Commands;

public class CreateBackInStockSubscriptionHandler(
    IBackInStockSubscriptionService backInStockSubscriptionService,
    IMapper mapper)
    : IRequestHandler<CreateBackInStockSubscriptionCommand, BackInStockSubscription>
{
    public async Task<BackInStockSubscription> Handle(CreateBackInStockSubscriptionCommand request,
        CancellationToken cancellationToken)
    {
        var backInStockSubscription = mapper.Map<BackInStockSubscription>(request);
        await backInStockSubscriptionService.SaveChangesAsync(new[] { backInStockSubscription });
        return backInStockSubscription;
    }
}
