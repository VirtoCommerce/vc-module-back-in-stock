using System;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.BackInStock.Core.Models;

public class BackInStockSubscription : AuditableEntity, ICloneable
{
    public string StoreId { get; set; }

    public string ProductId { get; set; }

    public string ProductName { get; set; }

    public string UserId { get; set; }

    public string MemberId { get; set; }

    public bool IsActive { get; set; }

    // TODO: Always null
    public DateTime? SentDate { get; set; }

    public object Clone()
    {
        return MemberwiseClone();
    }
}
