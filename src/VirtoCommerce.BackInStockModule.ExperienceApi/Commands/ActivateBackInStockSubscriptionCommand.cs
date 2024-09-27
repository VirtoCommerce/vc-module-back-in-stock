using VirtoCommerce.BackInStockModule.Core.Models;
using VirtoCommerce.Xapi.Core.Infrastructure;

namespace VirtoCommerce.BackInStockModule.ExperienceApi.Commands;

public class ActivateBackInStockSubscriptionCommand : ICommand<BackInStockSubscription>
{
    public string UserId { get; set; }

    public string ProductId { get; set; }

    public string StoreId { get; set; }
}
