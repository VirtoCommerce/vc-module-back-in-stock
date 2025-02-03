using GraphQL.Types;
using VirtoCommerce.BackInStock.ExperienceApi.Commands;

namespace VirtoCommerce.BackInStock.ExperienceApi.Schemas;

public class DeactivateBackInStockSubscriptionCommandType : InputObjectGraphType<DeactivateBackInStockSubscriptionCommand>
{
    public DeactivateBackInStockSubscriptionCommandType()
    {
        Field(x => x.StoreId);
        Field(x => x.ProductId);
    }
}
