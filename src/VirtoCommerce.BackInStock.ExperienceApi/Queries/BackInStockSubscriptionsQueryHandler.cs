using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.BackInStock.Core.Models;
using VirtoCommerce.BackInStock.Core.Services;
using VirtoCommerce.BackInStock.ExperienceApi.Extensions;
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
        criteria.StoreId = request.StoreId;
        criteria.ProductIds = request.ProductIds;
        criteria.UserId = request.UserId;
        criteria.IsActive = request.IsActive;

        if (!string.IsNullOrEmpty(request.Keyword))
        {
            var parseResult = phraseParser.Parse(request.Keyword);

            criteria.Keyword = parseResult.Keyword;

            // Term filters
            foreach (var term in parseResult.Filters.OfType<TermFilter>())
            {
                term.MapTo(criteria);
            }

            // Custom DateTime range filter
            parseResult.Filters
                .Get<RangeFilter>(nameof(BackInStockSubscription.SentDate))
                .MapTo(x => criteria.StartSentDate = x, x => criteria.EndSentDate = x);
        }

        return criteria;
    }
}
