using VirtoCommerce.CatalogModule.Core.Model;
using VirtoCommerce.CustomerModule.Core.Model;
using VirtoCommerce.NotificationsModule.Core.Model;

namespace VirtoCommerce.BackInStock.Core.Notifications;

public class BackInStockEmailNotification()
    : EmailNotification(nameof(BackInStockEmailNotification))
{
    public virtual string StoreUrl { get; set; }

    public virtual CatalogProduct Product { get; set; }

    public virtual Member Customer { get; set; }
}
