using System;
using System.ComponentModel.DataAnnotations;
using VirtoCommerce.BackInStock.Core.Models;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;

namespace VirtoCommerce.BackInStock.Data.Models;

public class BackInStockSubscriptionEntity : AuditableEntity, IDataEntity<BackInStockSubscriptionEntity, BackInStockSubscription>
{
    [Required]
    [StringLength(128)]
    public string StoreId { get; set; }

    [Required]
    [StringLength(128)]
    public string ProductId { get; set; }

    [StringLength(1024)]
    public string ProductName { get; set; }

    [Required]
    [StringLength(128)]
    public string UserId { get; set; }

    [StringLength(128)]
    public string MemberId { get; set; }

    public bool IsActive { get; set; }

    public DateTime? SentDate { get; set; }

    public virtual BackInStockSubscription ToModel(BackInStockSubscription model)
    {
        ArgumentNullException.ThrowIfNull(model);

        model.Id = Id;
        model.CreatedBy = CreatedBy;
        model.CreatedDate = CreatedDate;
        model.ModifiedBy = ModifiedBy;
        model.ModifiedDate = ModifiedDate;

        model.StoreId = StoreId;
        model.ProductId = ProductId;
        model.ProductName = ProductName;
        model.UserId = UserId;
        model.MemberId = MemberId;
        model.IsActive = IsActive;
        model.SentDate = SentDate;

        return model;
    }

    public virtual BackInStockSubscriptionEntity FromModel(BackInStockSubscription model, PrimaryKeyResolvingMap pkMap)
    {
        ArgumentNullException.ThrowIfNull(model);

        pkMap.AddPair(model, this);

        Id = model.Id;
        CreatedBy = model.CreatedBy;
        CreatedDate = model.CreatedDate;
        ModifiedBy = model.ModifiedBy;
        ModifiedDate = model.ModifiedDate;

        StoreId = model.StoreId;
        ProductId = model.ProductId;
        ProductName = model.ProductName;
        UserId = model.UserId;
        MemberId = model.MemberId;
        IsActive = model.IsActive;
        SentDate = model.SentDate;

        return this;
    }

    public virtual void Patch(BackInStockSubscriptionEntity target)
    {
        ArgumentNullException.ThrowIfNull(target);

        target.StoreId = StoreId;
        target.ProductId = ProductId;
        target.ProductName = ProductName;
        target.UserId = UserId;
        target.MemberId = MemberId;
        target.IsActive = IsActive;
        target.SentDate = SentDate;
    }
}
