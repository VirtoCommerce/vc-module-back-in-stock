using MediatR;
using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.BackInStock.Core.Models;
using VirtoCommerce.BackInStock.ExperienceApi.Schemas;
using VirtoCommerce.Xapi.Core.BaseQueries;

namespace VirtoCommerce.BackInStock.ExperienceApi.Queries;

public class BackInStockSubscriptionsQueryBuilder(IMediator mediator, IAuthorizationService authorizationService)
    : SearchQueryBuilder<BackInStockSubscriptionsQuery, BackInStockSubscriptionSearchResult, BackInStockSubscription,
        BackInStockSubscriptionType>(mediator, authorizationService)
{
    protected override string Name => "backInStockSubscription";
}
