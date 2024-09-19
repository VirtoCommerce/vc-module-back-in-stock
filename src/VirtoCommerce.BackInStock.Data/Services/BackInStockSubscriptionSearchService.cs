using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

public class BackInStockSubscriptionSearchService :
    SearchService<BackInStockSubscriptionSearchCriteria, BackInStockSubscriptionSearchResult, BackInStockSubscription,
        BackInStockSubscriptionEntity>,
    IBackInStockSubscriptionSearchService
{
    public BackInStockSubscriptionSearchService(
        Func<BackInStockSubscriptionRepository> repositoryFactory,
        IPlatformMemoryCache platformMemoryCache,
        IBackInStockSubscriptionService crudService,
        IOptions<CrudOptions> crudOptions)
        : base(repositoryFactory, platformMemoryCache, crudService, crudOptions)

    {
    }

    protected override IQueryable<BackInStockSubscriptionEntity> BuildQuery(IRepository repository,
        BackInStockSubscriptionSearchCriteria criteria)
    {
        var query = ((BackInStockSubscriptionRepository)repository).BackInStockSubscriptions;
        return query;
    }

    protected override IList<SortInfo> BuildSortExpression(BackInStockSubscriptionSearchCriteria criteria)
    {
        var sortInfos = criteria.SortInfos;

        if (sortInfos.IsNullOrEmpty())
        {
            sortInfos =
            [
                new SortInfo { SortColumn = nameof(BackInStockSubscriptionEntity.Id) },
            ];
        }

        return sortInfos;
    }
}
