using System.Collections.Generic;

namespace VirtoCommerce.BackInStock.Data.BackgroundJobs;

/// <summary>
/// Payload of the background job that notifies subscribers that products are back in stock.
/// </summary>
public class BackInStockNotificationJobPayload
{
    /// <summary>The products that came back in stock.</summary>
    public IList<string> ProductIds { get; set; } = [];
}
