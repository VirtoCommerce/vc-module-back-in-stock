using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.BackInStock.Core.BackgroundJobs;
using VirtoCommerce.InventoryModule.Core.Events;
using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.BackInStock.Data.Handlers;

public class InventoryChangedEventHandler(IBackInStockNotificationJobService backInStockNotificationJobService)
    : IEventHandler<InventoryChangedEvent>
{
    public Task Handle(InventoryChangedEvent inventoryChangedEvent)
    {
        var backInStockProductIds = inventoryChangedEvent.ChangedEntries
            .Where(x => x.OldEntry.InStockQuantity == 0 && x.NewEntry.InStockQuantity > 0 && !string.IsNullOrEmpty(x.NewEntry.ProductId))
            .Select(x => x.NewEntry.ProductId)
            .ToList();

        if (backInStockProductIds.Count > 0)
        {
            backInStockNotificationJobService.EnqueueProductBackInStockNotifications(backInStockProductIds);
        }

        return Task.CompletedTask;
    }
}
