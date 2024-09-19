using MediatR;
using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.BackInStock.Core.Models;
using VirtoCommerce.BackInStock.ExperienceApi.Schemas;
using VirtoCommerce.Xapi.Core.BaseQueries;

namespace VirtoCommerce.BackInStock.ExperienceApi.Commands;

public class CreateBackInStockSubscriptionCommandBuilder(IMediator mediator, IAuthorizationService authorizationService)
    : CommandBuilder<CreateBackInStockSubscriptionCommand,
        BackInStockSubscription, CreateBackInStockSubscriptionCommandType, BackInStockSubscriptionType>(mediator,
        authorizationService)
{
    protected override string Name => "createBackInStockSubscription";
}
