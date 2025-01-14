using System.Collections.Generic;

namespace VirtoCommerce.BackInStock.Core.BackgroundJobs;

public interface IBackInStockNotificationJobService
{
    void EnqueueProductBackInStockNotifications(IList<string> productIds);
}
