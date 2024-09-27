using VirtoCommerce.BackInStockModule.Core.Models;
using VirtoCommerce.Xapi.Core.Schemas;

namespace VirtoCommerce.BackInStockModule.ExperienceApi.Schemas;

public class BackInStockSubscriptionQueryType : ExtendableGraphType<BackInStockSubscription>
{
    public BackInStockSubscriptionQueryType()
    {
        Field(x => x.Id, nullable: true);
        Field(x => x.StoreId, nullable: true);
        Field(x => x.UserId, nullable: true);
        Field(x => x.Triggered, nullable: true);
        Field(x => x.IsActive, nullable: true);
        Field(x => x.ProductId, nullable: true);
    }
}
