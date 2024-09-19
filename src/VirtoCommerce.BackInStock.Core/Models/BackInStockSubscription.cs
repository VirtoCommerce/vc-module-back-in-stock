using System;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.BackInStock.Core.Models
{
    public class BackInStockSubscription : AuditableEntity, ICloneable
    {
        /// <summary>
        /// User id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Product id
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// Store id
        /// </summary>
        public string StoreId { get; set; }

        /// <summary>
        /// Last triggered time
        /// </summary>
        public DateTime? Triggered { get; set; }

        /// <summary>
        /// Is subscription active
        /// </summary>
        public bool IsActive { get; set; }

        public object Clone()
        {
            return MemberwiseClone() as BackInStockSubscription ?? throw new InvalidOperationException();
        }
    }
}
