using System.Threading.Tasks;
using GraphQL;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.BackInStockModule.Core.Models;
using VirtoCommerce.BackInStockModule.ExperienceApi.Authorization;
using VirtoCommerce.BackInStockModule.ExperienceApi.Schemas;
using VirtoCommerce.Xapi.Core.BaseQueries;
using VirtoCommerce.Xapi.Core.Extensions;

namespace VirtoCommerce.BackInStockModule.ExperienceApi.Commands;

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
