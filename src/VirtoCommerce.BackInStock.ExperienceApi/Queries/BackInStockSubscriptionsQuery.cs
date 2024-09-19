using System;
using System.Collections.Generic;
using GraphQL;
using GraphQL.Types;
using VirtoCommerce.BackInStock.Core.Models;
using VirtoCommerce.Xapi.Core.BaseQueries;

namespace VirtoCommerce.BackInStock.ExperienceApi.Queries;

public class BackInStockSubscriptionsQuery : SearchQuery<BackInStockSubscriptionSearchResult>
{
    public string UserId { get; set; }
    public string StoreId { get; set; }

    public string ProductId { get; set; }

    public bool IsActive { get; set; }

    public DateTime StartTriggeredDate { get; set; }

    public DateTime EndTriggeredDate { get; set; }

    public string Filter { get; set; }

    public override IEnumerable<QueryArgument> GetArguments()
    {
        foreach (var argument in base.GetArguments())
        {
            yield return argument;
        }

        yield return Argument<NonNullGraphType<StringGraphType>>(nameof(StoreId));
        yield return Argument<NonNullGraphType<StringGraphType>>(nameof(UserId));
        yield return Argument<NonNullGraphType<StringGraphType>>(nameof(ProductId));
        yield return Argument<NonNullGraphType<BooleanGraphType>>(nameof(IsActive));
        yield return Argument<StringGraphType>(nameof(Filter));
    }

    public override void Map(IResolveFieldContext context)
    {
        base.Map(context);

        StoreId = context.GetArgument<string>(nameof(StoreId));
        ProductId = context.GetArgument<string>(nameof(ProductId));
        UserId = context.GetArgument<string>(nameof(UserId));
        IsActive = context.GetArgument<bool>(nameof(IsActive));
        Filter = context.GetArgument<string>(nameof(Filter));
    }
}
