using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using VirtoCommerce.BackInStockModule.Core.Models;
using VirtoCommerce.BackInStockModule.Core.Services;
using VirtoCommerce.BackInStockModule.Data.Models;
using VirtoCommerce.BackInStockModule.Data.Repositories;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.GenericCrud;
using VirtoCommerce.Platform.Data.GenericCrud;

namespace VirtoCommerce.BackInStockModule.Data.Services;

public class BackInStockSubscriptionSearchService(
    Func<IBackInStockSubscriptionRepository> repositoryFactory,
    IPlatformMemoryCache platformMemoryCache,
    IBackInStockSubscriptionService crudService,
    IOptions<CrudOptions> crudOptions)
    : SearchService<BackInStockSubscriptionSearchCriteria, BackInStockSubscriptionSearchResult,
            BackInStockSubscription,
            BackInStockSubscriptionEntity>(repositoryFactory, platformMemoryCache, crudService, crudOptions),
        IBackInStockSubscriptionSearchService
{
    protected override IQueryable<BackInStockSubscriptionEntity> BuildQuery(IRepository repository,
        BackInStockSubscriptionSearchCriteria criteria)
    {
        var query = ((BackInStockSubscriptionRepository)repository).BackInStockSubscriptions;

        if (criteria.ProductId != null)
        {
            query = query.Where(x => x.ProductId == criteria.ProductId);
        }

        if (criteria.UserId != null)
        {
            query = query.Where(x => x.UserId == criteria.UserId);
        }

        if (criteria.StoreId != null)
        {
            query = query.Where(x => x.StoreId == criteria.StoreId);
        }

        if (criteria.StartTriggeredDate != null)
        {
            query = query.Where(x => x.Triggered >= criteria.StartTriggeredDate);
        }

        if (criteria.EndTriggeredDate != null)
        {
            query = query.Where(x => x.Triggered <= criteria.EndTriggeredDate);
        }

        if (criteria.Keyword != null)
        {
            query = query.Where(x =>
                x.UserId.Contains(criteria.Keyword) || x.ProductId.Contains(criteria.Keyword) ||
                x.StoreId.Contains(criteria.Keyword));
        }

        return query;
    }

    protected override IList<SortInfo> BuildSortExpression(BackInStockSubscriptionSearchCriteria criteria)
    {
        var sortInfos = criteria.SortInfos;

        if (sortInfos.IsNullOrEmpty())
        {
            sortInfos =
            [
                new SortInfo
                {
                    SortColumn = nameof(BackInStockSubscriptionEntity.CreatedDate),
                    SortDirection = SortDirection.Descending
                },
                new SortInfo { SortColumn = nameof(BackInStockSubscriptionEntity.Id) },
            ];
        }

        return sortInfos;
    }
}
