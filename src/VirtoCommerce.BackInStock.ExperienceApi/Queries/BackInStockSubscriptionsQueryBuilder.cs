using System.Threading.Tasks;
using GraphQL;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.BackInStock.Core.Models;
using VirtoCommerce.BackInStock.ExperienceApi.Schemas;
using VirtoCommerce.Xapi.Core.BaseQueries;
using VirtoCommerce.Xapi.Core.Extensions;

namespace VirtoCommerce.BackInStock.ExperienceApi.Queries;

public class BackInStockSubscriptionsQueryBuilder(IMediator mediator, IAuthorizationService authorizationService)
    : SearchQueryBuilder<BackInStockSubscriptionsQuery, BackInStockSubscriptionSearchResult, BackInStockSubscription,
        BackInStockSubscriptionQueryType>(mediator, authorizationService)
{
    protected override string Name => "getBackInStockSubscriptions";

    protected override async Task BeforeMediatorSend(IResolveFieldContext<object> context, BackInStockSubscriptionsQuery request)
    {
        //await Authorize(context, null, new BackInStockAuthorizationRequirement());

        request.UserId = context.GetCurrentUserId();

        await base.BeforeMediatorSend(context, request);
    }
}
