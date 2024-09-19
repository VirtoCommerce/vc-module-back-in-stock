using System;
using VirtoCommerce.BackInStock.Core.Models;
using VirtoCommerce.Xapi.Core.Infrastructure;

namespace VirtoCommerce.BackInStock.ExperienceApi.Commands;

public class ActivateBackInStockSubscriptionCommand : ICommand<BackInStockSubscription>
{
    /// <summary>
    /// User id
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// Product id
    /// </summary>
    public string ProductId { get; set; }

    /// <summary>
    /// Store id
    /// </summary>
    public string StoreId { get; set; }
}
