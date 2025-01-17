using System.Collections.Generic;

namespace VirtoCommerce.BackInStock.Core.BackgroundJobs;

public interface IBackInStockNotificationJob
{
    void EnqueueProductBackInStockNotifications(IList<string> productIds);
}
