using VirtoCommerce.BackInStock.Core.Models;
using VirtoCommerce.Xapi.Core.Infrastructure;

namespace VirtoCommerce.BackInStock.ExperienceApi.Commands;

public class DeactivateBackInStockSubscriptionCommand : ICommand<BackInStockSubscription>
{
    public string StoreId { get; set; }

    public string ProductId { get; set; }

    public string UserId { get; set; }
}
