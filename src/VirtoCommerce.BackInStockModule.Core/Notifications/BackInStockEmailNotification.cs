using VirtoCommerce.CatalogModule.Core.Model;
using VirtoCommerce.CustomerModule.Core.Model;
using VirtoCommerce.NotificationsModule.Core.Model;

namespace VirtoCommerce.BackInStockModule.Core.Notifications
{
    public class BackInStockEmailNotification : EmailNotification
    {
        public BackInStockEmailNotification() : base(nameof(BackInStockEmailNotification))
        {
        }

        public virtual Member Customer { get; set; }

        public virtual CatalogProduct Item { get; set; }
    }
}
