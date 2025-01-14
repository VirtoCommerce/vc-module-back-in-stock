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

    public IList<string> ProductIds { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? StartTriggeredDate { get; set; }

    public DateTime? EndTriggeredDate { get; set; }

    public string Filter { get; set; }

    public override IEnumerable<QueryArgument> GetArguments()
    {
        foreach (var argument in base.GetArguments())
        {
            yield return argument;
        }

        yield return Argument<StringGraphType>(nameof(UserId));
        yield return Argument<StringGraphType>(nameof(StoreId));
        yield return Argument<ListGraphType<StringGraphType>>(nameof(ProductIds));
        yield return Argument<BooleanGraphType>(nameof(IsActive));
        yield return Argument<DateTimeGraphType>(nameof(StartTriggeredDate));
        yield return Argument<DateTimeGraphType>(nameof(EndTriggeredDate));
        yield return Argument<StringGraphType>(nameof(Filter));
    }

    public override void Map(IResolveFieldContext context)
    {
        base.Map(context);

        UserId = context.GetArgument<string>(nameof(UserId));
        StoreId = context.GetArgument<string>(nameof(StoreId));
        ProductIds = context.GetArgument<IList<string>>(nameof(ProductIds));
        IsActive = context.GetArgument<bool?>(nameof(IsActive));
        StartTriggeredDate = context.GetArgument<DateTime?>(nameof(StartTriggeredDate));
        EndTriggeredDate = context.GetArgument<DateTime?>(nameof(EndTriggeredDate));
        Filter = context.GetArgument<string>(nameof(Filter));
    }
}
