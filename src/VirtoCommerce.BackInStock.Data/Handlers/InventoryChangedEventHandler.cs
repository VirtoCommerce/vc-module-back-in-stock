using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.BackInStock.Core.BackgroundJobs;
using VirtoCommerce.InventoryModule.Core.Events;
using VirtoCommerce.InventoryModule.Core.Model;
using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.BackInStock.Data.Handlers;

public class InventoryChangedEventHandler(IBackInStockNotificationJob backInStockNotificationJob)
    : IEventHandler<InventoryChangedEvent>
{
    public Task Handle(InventoryChangedEvent inventoryChangedEvent)
    {
        var backInStockProductIds = inventoryChangedEvent.ChangedEntries
            .Where(x => !IsInStock(x.OldEntry) && IsInStock(x.NewEntry) && !string.IsNullOrEmpty(x.NewEntry.ProductId))
            .Select(x => x.NewEntry.ProductId)
            .ToList();

        if (backInStockProductIds.Count > 0)
        {
            backInStockNotificationJob.EnqueueProductBackInStockNotifications(backInStockProductIds);
        }

        return Task.CompletedTask;
    }

    // TODO: Use the same conditions as in the XCatalog module
    private static bool IsInStock(InventoryInfo inventory)
    {
        return inventory.AllowPreorder ||
               inventory.AllowBackorder ||
               inventory.InStockQuantity > inventory.ReservedQuantity;
    }
}
