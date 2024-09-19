using GraphQL.Types;
using VirtoCommerce.BackInStock.ExperienceApi.Commands;

namespace VirtoCommerce.BackInStock.ExperienceApi.Schemas;

public class CreateBackInStockSubscriptionCommandType : InputObjectGraphType<CreateBackInStockSubscriptionCommand>
{
    public CreateBackInStockSubscriptionCommandType()
    {
        Field(x => x.UserId);
        Field(x => x.StoreId);
        Field(x => x.ProductId);
        Field(x => x.IsActive);
        Field(x => x.Triggered);
    }
}
