using VirtoCommerce.BackInStock.Core.Models;
using VirtoCommerce.Xapi.Core.Schemas;

namespace VirtoCommerce.BackInStock.ExperienceApi.Schemas;

public class BackInStockSubscriptionType : ExtendableGraphType<BackInStockSubscription>
{
    public BackInStockSubscriptionType()
    {
        Name = "BackInStockSubscription";

        Field(x => x.Id);
        Field(x => x.StoreId);
        Field(x => x.UserId);
        Field(x => x.Triggered, nullable: true);
        Field(x => x.IsActive);
        Field(x => x.ProductId);
    }
}
