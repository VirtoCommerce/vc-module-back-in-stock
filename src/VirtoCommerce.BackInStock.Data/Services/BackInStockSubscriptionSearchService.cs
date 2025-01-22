using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using VirtoCommerce.BackInStock.Core.Models;
using VirtoCommerce.BackInStock.Core.Services;
using VirtoCommerce.BackInStock.Data.Models;
using VirtoCommerce.BackInStock.Data.Repositories;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.GenericCrud;
using VirtoCommerce.Platform.Data.GenericCrud;

namespace VirtoCommerce.BackInStock.Data.Services;

public class BackInStockSubscriptionSearchService(
    Func<IBackInStockRepository> repositoryFactory,
    IPlatformMemoryCache platformMemoryCache,
    IBackInStockSubscriptionService crudService,
    IOptions<CrudOptions> crudOptions)
    : SearchService<BackInStockSubscriptionSearchCriteria, BackInStockSubscriptionSearchResult, BackInStockSubscription, BackInStockSubscriptionEntity>
        (repositoryFactory, platformMemoryCache, crudService, crudOptions),
        IBackInStockSubscriptionSearchService
{
    protected override IQueryable<BackInStockSubscriptionEntity> BuildQuery(IRepository repository, BackInStockSubscriptionSearchCriteria criteria)
    {
        var query = ((IBackInStockRepository)repository).BackInStockSubscriptions;

        if (criteria.StoreId != null)
        {
            query = query.Where(x => x.StoreId == criteria.StoreId);
        }

        if (criteria.ProductIds != null)
        {
            query = criteria.ProductIds.Count == 1
                ? query.Where(x => x.ProductId == criteria.ProductIds.First())
                : query.Where(x => criteria.ProductIds.Contains(x.ProductId));
        }

        if (criteria.UserId != null)
        {
            query = query.Where(x => x.UserId == criteria.UserId);
        }

        if (criteria.MemberId != null)
        {
            query = query.Where(x => x.MemberId == criteria.MemberId);
        }

        if (criteria.IsActive != null)
        {
            query = query.Where(x => x.IsActive == criteria.IsActive);
        }

        if (criteria.Keyword != null)
        {
            query = query.Where(x =>
                x.ProductName.Contains(criteria.Keyword) ||
                x.ProductId.Contains(criteria.Keyword) ||
                x.UserId.Contains(criteria.Keyword) ||
                x.MemberId.Contains(criteria.Keyword) ||
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
                new SortInfo { SortColumn = nameof(BackInStockSubscriptionEntity.ProductName) },
                new SortInfo { SortColumn = nameof(BackInStockSubscriptionEntity.Id) },
            ];
        }

        return sortInfos;
    }
}
