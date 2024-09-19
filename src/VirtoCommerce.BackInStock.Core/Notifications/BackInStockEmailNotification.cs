using VirtoCommerce.CustomerModule.Core.Model;
using VirtoCommerce.NotificationsModule.Core.Model;
using VirtoCommerce.OrdersModule.Core.Model;

namespace VirtoCommerce.BackInStock.Core.Notifications
{
    public class BackInStockEmailNotification : EmailNotification
    {
        public BackInStockEmailNotification() : base(nameof(BackInStockEmailNotification))
        {
        }

        public string RequestId { get; set; }
        public virtual Member Customer { get; set; }
        public virtual LineItem Item { get; set; }
    }
}
