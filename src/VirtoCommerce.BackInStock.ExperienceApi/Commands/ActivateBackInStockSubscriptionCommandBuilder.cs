using System.Threading.Tasks;
using GraphQL;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.BackInStock.Core.Models;
using VirtoCommerce.BackInStock.ExperienceApi.Schemas;
using VirtoCommerce.Xapi.Core.BaseQueries;
using VirtoCommerce.Xapi.Core.Extensions;

namespace VirtoCommerce.BackInStock.ExperienceApi.Commands;

public class ActivateBackInStockSubscriptionCommandBuilder(IMediator mediator, IAuthorizationService authorizationService)
    : CommandBuilder<ActivateBackInStockSubscriptionCommand, BackInStockSubscription, ActivateBackInStockSubscriptionCommandType,
        BackInStockSubscriptionType>(mediator, authorizationService)
{
    protected override string Name => "activateBackInStockSubscription";

    protected override async Task BeforeMediatorSend(IResolveFieldContext<object> context, ActivateBackInStockSubscriptionCommand request)
    {
        //await Authorize(context, null, new BackInStockAuthorizationRequirement());

        request.UserId = context.GetCurrentUserId();

        await base.BeforeMediatorSend(context, request);
    }
}
