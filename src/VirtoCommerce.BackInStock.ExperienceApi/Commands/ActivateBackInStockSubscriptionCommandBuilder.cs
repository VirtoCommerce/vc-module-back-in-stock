using MediatR;
using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.BackInStock.Core.Models;
using VirtoCommerce.BackInStock.ExperienceApi.Schemas;
using VirtoCommerce.Xapi.Core.BaseQueries;

namespace VirtoCommerce.BackInStock.ExperienceApi.Commands;

public class ActivateBackInStockSubscriptionCommandBuilder(IMediator mediator, IAuthorizationService authorizationService)
    : CommandBuilder<ActivateBackInStockSubscriptionCommand,
        BackInStockSubscription, ActivateBackInStockSubscriptionCommandType, BackInStockSubscriptionType>(mediator,
        authorizationService)
{
    protected override string Name => "activateBackInStockSubscription";
}
