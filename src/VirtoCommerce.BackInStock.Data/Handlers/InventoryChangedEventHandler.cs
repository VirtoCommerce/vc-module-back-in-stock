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
        var inStockProducts = inventoryChangedEvent.ChangedEntries
            .Where(x => x.OldEntry.InStockQuantity == 0 && x.NewEntry.InStockQuantity > 0)
            .Select(x => x.NewEntry.ProductId)
            .ToList();

        backInStockNotificationJobService.EnqueueProductBackInStockNotifications(inStockProducts);

        return Task.CompletedTask;
    }
}
