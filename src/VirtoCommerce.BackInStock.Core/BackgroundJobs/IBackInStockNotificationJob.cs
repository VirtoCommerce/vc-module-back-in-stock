using System.Collections.Generic;

namespace VirtoCommerce.BackInStock.Core.BackgroundJobs;

public interface IBackInStockNotificationJob
{
    void Enqueue(IList<string> productIds);
}
