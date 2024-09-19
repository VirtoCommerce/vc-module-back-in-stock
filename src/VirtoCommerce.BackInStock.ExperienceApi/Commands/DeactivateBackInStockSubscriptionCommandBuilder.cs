using MediatR;
using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.BackInStock.Core.Models;
using VirtoCommerce.BackInStock.ExperienceApi.Schemas;
using VirtoCommerce.Xapi.Core.BaseQueries;

namespace VirtoCommerce.BackInStock.ExperienceApi.Commands;

public class DeactivateBackInStockSubscriptionCommandBuilder(
    IMediator mediator,
    IAuthorizationService authorizationService)
    : CommandBuilder<DeactivateBackInStockSubscriptionCommand,
        BackInStockSubscription, DeactivateBackInStockSubscriptionCommandType, BackInStockSubscriptionType>(mediator,
        authorizationService)
{
    protected override string Name => "deactivateBackInStockSubscription";
}
