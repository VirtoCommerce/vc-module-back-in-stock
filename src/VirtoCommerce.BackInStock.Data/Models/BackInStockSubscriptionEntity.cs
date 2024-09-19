using System;
using System.ComponentModel.DataAnnotations;
using VirtoCommerce.BackInStock.Core.Models;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;

namespace VirtoCommerce.BackInStock.Data.Models
{
    public sealed class BackInStockSubscriptionEntity : AuditableEntity,
        IDataEntity<BackInStockSubscriptionEntity, BackInStockSubscription>
    {
        /// <summary>
        /// User Id
        /// </summary>
        [StringLength(128)]
        [Required]
        public string UserId { get; set; }

        /// <summary>
        /// Store Id
        /// </summary>
        [StringLength(128)]
        [Required]
        public string StoreId { get; set; }

        /// <summary>
        /// Product id
        /// </summary>
        [StringLength(128)]
        [Required]
        public string ProductId { get; set; }

        /// <summary>
        /// Last triggered time
        /// </summary>
        public DateTime? Triggered { get; set; }

        /// <summary>
        /// Is subscription active
        /// </summary>
        public bool IsActive { get; set; }

        public BackInStockSubscription ToModel(BackInStockSubscription model)
        {
            ArgumentNullException.ThrowIfNull(model);

            model.Id = Id;
            model.CreatedBy = CreatedBy;
            model.CreatedDate = CreatedDate;
            model.ModifiedBy = ModifiedBy;
            model.ModifiedDate = ModifiedDate;

            model.UserId = UserId;
            model.StoreId = StoreId;
            model.ProductId = ProductId;
            model.Triggered = Triggered;
            model.IsActive = IsActive;

            return model;
        }

        public BackInStockSubscriptionEntity FromModel(BackInStockSubscription model,
            PrimaryKeyResolvingMap pkMap)
        {
            ArgumentNullException.ThrowIfNull(model);

            pkMap.AddPair(model, this);

            Id = model.Id;
            CreatedBy = model.CreatedBy;
            CreatedDate = model.CreatedDate;
            ModifiedBy = model.ModifiedBy;
            ModifiedDate = model.ModifiedDate;

            UserId = model.UserId;
            StoreId = model.StoreId;
            ProductId = model.ProductId;
            Triggered = model.Triggered;
            IsActive = model.IsActive;

            return this;
        }

        public void Patch(BackInStockSubscriptionEntity target)
        {
            ArgumentNullException.ThrowIfNull(target);

            target.UserId = UserId;
            target.StoreId = StoreId;
            target.ProductId = ProductId;
            target.Triggered = Triggered;
            target.IsActive = IsActive;
        }
    }
}
