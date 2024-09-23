using GraphQL.Types;
using VirtoCommerce.BackInStockModule.ExperienceApi.Commands;

namespace VirtoCommerce.BackInStockModule.ExperienceApi.Schemas;

public class
    DeactivateBackInStockSubscriptionCommandType : InputObjectGraphType<DeactivateBackInStockSubscriptionCommand>
{
    public DeactivateBackInStockSubscriptionCommandType()
    {
        Field(x => x.UserId);
        Field(x => x.StoreId);
        Field(x => x.ProductId);
    }
}
