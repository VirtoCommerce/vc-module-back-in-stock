using VirtoCommerce.BackInStock.Core.Models;
using VirtoCommerce.Xapi.Core.Schemas;

namespace VirtoCommerce.BackInStock.ExperienceApi.Schemas;

public class BackInStockSubscriptionType : ExtendableGraphType<BackInStockSubscription>
{
    public BackInStockSubscriptionType()
    {
        Field(x => x.Id);
        Field(x => x.StoreId);
        Field(x => x.ProductId);
        Field(x => x.ProductCode, nullable: true);
        Field(x => x.ProductName, nullable: true);
        Field(x => x.UserId);
        Field(x => x.MemberId, nullable: true);
        Field(x => x.IsActive);
    }
}
