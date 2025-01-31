using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.BackInStock.Core.BackgroundJobs;
using VirtoCommerce.InventoryModule.Core.Events;
using VirtoCommerce.InventoryModule.Core.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.BackInStock.Data.Handlers;

public class InventoryChangedEventHandler(IBackInStockNotificationJob backInStockNotificationJob)
    : IEventHandler<InventoryChangedEvent>
{
    public Task Handle(InventoryChangedEvent inventoryChangedEvent)
    {
        var backInStockProductIds = inventoryChangedEvent.ChangedEntries
            .Where(x => !string.IsNullOrEmpty(x.NewEntry.ProductId) && IsInStock(x.NewEntry) && (
                x.EntryState == EntryState.Added ||
                x.EntryState == EntryState.Modified && !IsInStock(x.OldEntry)))
            .Select(x => x.NewEntry.ProductId)
            .Distinct()
            .ToList();

        if (backInStockProductIds.Count > 0)
        {
            backInStockNotificationJob.Enqueue(backInStockProductIds);
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
