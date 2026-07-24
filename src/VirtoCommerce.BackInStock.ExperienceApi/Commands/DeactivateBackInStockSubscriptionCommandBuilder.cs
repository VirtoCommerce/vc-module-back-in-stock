using System;
using System.Threading.Tasks;
using GraphQL;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.BackInStock.Core.Models;
using VirtoCommerce.BackInStock.ExperienceApi.Authorization;
using VirtoCommerce.BackInStock.ExperienceApi.Schemas;
using VirtoCommerce.Xapi.Core.BaseQueries;
using VirtoCommerce.Xapi.Core.Extensions;

namespace VirtoCommerce.BackInStock.ExperienceApi.Commands;

public class DeactivateBackInStockSubscriptionCommandBuilder(IAuthorizationService authorizationService)
    : CommandBuilder<DeactivateBackInStockSubscriptionCommand, BackInStockSubscription, DeactivateBackInStockSubscriptionCommandType, BackInStockSubscriptionType>
        (authorizationService)
{
    [Obsolete("Use the constructor without IMediator. The mediator is resolved from context.RequestServices per request.", DiagnosticId = "VC0015", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public DeactivateBackInStockSubscriptionCommandBuilder(IMediator mediator, IAuthorizationService authorizationService)
        : this(authorizationService)
    {
    }

    protected override string Name => "deactivateBackInStockSubscription";

    protected override async Task BeforeMediatorSend(IResolveFieldContext<object> context, DeactivateBackInStockSubscriptionCommand request)
    {
        await base.BeforeMediatorSend(context, request);
        await Authorize(context, request, new BackInStockAuthorizationRequirement());

        request.UserId = context.GetCurrentUserId();
    }
}
