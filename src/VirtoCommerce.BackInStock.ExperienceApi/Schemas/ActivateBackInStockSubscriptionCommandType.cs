using GraphQL.Types;
using VirtoCommerce.BackInStock.ExperienceApi.Commands;

namespace VirtoCommerce.BackInStock.ExperienceApi.Schemas;

public class ActivateBackInStockSubscriptionCommandType : InputObjectGraphType<ActivateBackInStockSubscriptionCommand>
{
    public ActivateBackInStockSubscriptionCommandType()
    {
        Field(x => x.UserId);
        Field(x => x.StoreId);
        Field(x => x.ProductId);
    }
}
