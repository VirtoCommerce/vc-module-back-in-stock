using System;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.BackInStockModule.Core.Models;

public class BackInStockSubscription : AuditableEntity, ICloneable
{
    public string UserId { get; set; }

    public string ProductId { get; set; }

    public string StoreId { get; set; }

    public DateTime? Triggered { get; set; }

    public bool IsActive { get; set; }

    public object Clone()
    {
        return (BackInStockSubscription)MemberwiseClone();
    }
}
