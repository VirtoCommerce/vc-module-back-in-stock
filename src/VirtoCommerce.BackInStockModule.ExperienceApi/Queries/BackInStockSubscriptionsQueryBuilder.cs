using MediatR;
using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.BackInStockModule.Core.Models;
using VirtoCommerce.BackInStockModule.ExperienceApi.Schemas;
using VirtoCommerce.Xapi.Core.BaseQueries;

namespace VirtoCommerce.BackInStockModule.ExperienceApi.Queries;

public class BackInStockSubscriptionsQueryBuilder(IMediator mediator, IAuthorizationService authorizationService)
    : SearchQueryBuilder<BackInStockSubscriptionsQuery, BackInStockSubscriptionSearchResult, BackInStockSubscription,
        BackInStockSubscriptionType>(mediator, authorizationService)
{
    protected override string Name => "backInStockSubscription";
}
