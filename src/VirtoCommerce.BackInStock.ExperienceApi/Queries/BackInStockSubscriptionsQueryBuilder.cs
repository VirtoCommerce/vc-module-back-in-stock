using System.Threading.Tasks;
using GraphQL;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.BackInStock.Core.Models;
using VirtoCommerce.BackInStock.ExperienceApi.Authorization;
using VirtoCommerce.BackInStock.ExperienceApi.Schemas;
using VirtoCommerce.Xapi.Core.BaseQueries;

namespace VirtoCommerce.BackInStock.ExperienceApi.Queries;

public class BackInStockSubscriptionsQueryBuilder(IMediator mediator, IAuthorizationService authorizationService)
    : SearchQueryBuilder<BackInStockSubscriptionsQuery, BackInStockSubscriptionSearchResult, BackInStockSubscription, BackInStockSubscriptionType>
        (mediator, authorizationService)
{
    protected override string Name => "backInStockSubscriptions";

    protected override async Task BeforeMediatorSend(IResolveFieldContext<object> context, BackInStockSubscriptionsQuery request)
    {
        await base.BeforeMediatorSend(context, request);
        await Authorize(context, request, new BackInStockAuthorizationRequirement());
    }
}
