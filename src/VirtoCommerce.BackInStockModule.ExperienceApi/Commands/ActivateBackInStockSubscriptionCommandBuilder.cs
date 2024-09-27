using MediatR;
using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.BackInStockModule.Core.Models;
using VirtoCommerce.BackInStockModule.ExperienceApi.Schemas;
using VirtoCommerce.Xapi.Core.BaseQueries;

namespace VirtoCommerce.BackInStockModule.ExperienceApi.Commands;

public class ActivateBackInStockSubscriptionCommandBuilder(IMediator mediator, IAuthorizationService authorizationService)
    : CommandBuilder<ActivateBackInStockSubscriptionCommand,
        BackInStockSubscription, ActivateBackInStockSubscriptionCommandType, BackInStockSubscriptionType>(mediator,
        authorizationService)
{
    protected override string Name => "activateBackInStockSubscription";
}
