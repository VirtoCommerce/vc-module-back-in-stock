using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.BackInStock.Core.Models;
using VirtoCommerce.BackInStock.Core.Services;
using VirtoCommerce.SearchModule.Core.Model;
using VirtoCommerce.SearchModule.Core.Services;
using VirtoCommerce.Xapi.Core.Index;
using VirtoCommerce.Xapi.Core.Infrastructure;

namespace VirtoCommerce.BackInStock.ExperienceApi.Queries;

public class BackInStockQueryHandler(
    IBackInStockSubscriptionSearchService searchService,
    ISearchPhraseParser phraseParser)
    : IQueryHandler<BackInStockSubscriptionsQuery, BackInStockSubscriptionSearchResult>
{
    public virtual async Task<BackInStockSubscriptionSearchResult> Handle(BackInStockSubscriptionsQuery request, CancellationToken cancellationToken)
    {
        var criteria = GetSearchCriteria(request);
        var result = await searchService.SearchAsync(criteria);

        return result;
    }

    protected virtual BackInStockSubscriptionSearchCriteria GetSearchCriteria(BackInStockSubscriptionsQuery request)
    {
        var criteria = request.GetSearchCriteria<BackInStockSubscriptionSearchCriteria>();

        if (!string.IsNullOrEmpty(request.Keyword))
        {
            ApplyFilter(criteria, request.Keyword);
        }

        criteria.StoreId = request.StoreId;
        criteria.ProductIds = request.ProductIds;
        criteria.UserId = request.UserId;
        criteria.IsActive = request.IsActive;

        return criteria;
    }

    protected virtual void ApplyFilter(BackInStockSubscriptionSearchCriteria criteria, string filter)
    {
        var parseResult = phraseParser.Parse(filter);

        criteria.Keyword = parseResult.Keyword;

        // Term filters
        foreach (var term in parseResult.Filters.OfType<TermFilter>())
        {
            term.MapTo(criteria);
        }
    }
}
