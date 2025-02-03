using System.Collections.Generic;
using GraphQL;
using GraphQL.Types;
using VirtoCommerce.BackInStock.Core.Models;
using VirtoCommerce.Xapi.Core.BaseQueries;
using VirtoCommerce.Xapi.Core.Extensions;

namespace VirtoCommerce.BackInStock.ExperienceApi.Queries;

public class BackInStockSubscriptionsQuery : SearchQuery<BackInStockSubscriptionSearchResult>
{
    public string StoreId { get; set; }

    public IList<string> ProductIds { get; set; }

    public string UserId { get; set; }

    public bool? IsActive { get; set; }

    public override IEnumerable<QueryArgument> GetArguments()
    {
        foreach (var argument in base.GetArguments())
        {
            yield return argument;
        }

        yield return Argument<StringGraphType>(nameof(StoreId));
        yield return Argument<ListGraphType<StringGraphType>>(nameof(ProductIds));
        yield return Argument<BooleanGraphType>(nameof(IsActive));
    }

    public override void Map(IResolveFieldContext context)
    {
        base.Map(context);

        UserId = context.GetCurrentUserId();

        StoreId = context.GetArgument<string>(nameof(StoreId));
        ProductIds = context.GetArgument<IList<string>>(nameof(ProductIds));
        IsActive = context.GetArgument<bool?>(nameof(IsActive));
    }
}
