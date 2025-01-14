using VirtoCommerce.BackInStock.Core.Models;
using VirtoCommerce.Xapi.Core.Infrastructure;

namespace VirtoCommerce.BackInStock.ExperienceApi.Commands;

public class ActivateBackInStockSubscriptionCommand : ICommand<BackInStockSubscription>
{
    public string UserId { get; set; }

    public string ProductId { get; set; }

    public string StoreId { get; set; }
}
