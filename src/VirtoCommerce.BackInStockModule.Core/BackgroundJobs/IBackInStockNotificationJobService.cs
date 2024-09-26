using System.Collections.Generic;

namespace VirtoCommerce.BackInStockModule.Core.BackgroundJobs;

public interface IBackInStockNotificationJobService
{
    void EnqueueProductBackInStockNotifications(IList<string> productIds);
}
