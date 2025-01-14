using System.Threading.Tasks;
using GraphQL;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.BackInStock.Core.Models;
using VirtoCommerce.BackInStock.ExperienceApi.Schemas;
using VirtoCommerce.Xapi.Core.BaseQueries;
using VirtoCommerce.Xapi.Core.Extensions;

namespace VirtoCommerce.BackInStock.ExperienceApi.Commands;

public class DeactivateBackInStockSubscriptionCommandBuilder(IMediator mediator, IAuthorizationService authorizationService)
    : CommandBuilder<DeactivateBackInStockSubscriptionCommand, BackInStockSubscription, DeactivateBackInStockSubscriptionCommandType,
        BackInStockSubscriptionType>(mediator, authorizationService)
{
    protected override string Name => "deactivateBackInStockSubscription";

    protected override async Task BeforeMediatorSend(IResolveFieldContext<object> context, DeactivateBackInStockSubscriptionCommand request)
    {
        //await Authorize(context, null, new BackInStockAuthorizationRequirement());

        request.UserId = context.GetCurrentUserId();

        await base.BeforeMediatorSend(context, request);
    }
}
