using System.Threading.Tasks;
using GraphQL;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.BackInStockModule.Core.Models;
using VirtoCommerce.BackInStockModule.ExperienceApi.Authorization;
using VirtoCommerce.BackInStockModule.ExperienceApi.Schemas;
using VirtoCommerce.Xapi.Core.BaseQueries;
using VirtoCommerce.Xapi.Core.Extensions;

namespace VirtoCommerce.BackInStockModule.ExperienceApi.Queries;

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
