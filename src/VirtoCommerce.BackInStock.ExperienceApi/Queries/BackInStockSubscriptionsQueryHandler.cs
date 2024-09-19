using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.BackInStock.Core.Models;
using VirtoCommerce.BackInStock.Core.Services;
using VirtoCommerce.BackInStock.ExperienceApi.Extensions;
using VirtoCommerce.Xapi.Core.Index;
using VirtoCommerce.Xapi.Core.Infrastructure;
using VirtoCommerce.SearchModule.Core.Model;
using VirtoCommerce.SearchModule.Core.Services;

namespace VirtoCommerce.BackInStock.ExperienceApi.Queries;

public class BackInStockQueryHandler : IQueryHandler<BackInStockSubscriptionsQuery, BackInStockSubscriptionSearchResult>
{
    private readonly IBackInStockSubscriptionSearchService _backInStockSubscriptionSearchService;
    private readonly ISearchPhraseParser _phraseParser;

    public BackInStockQueryHandler(
        IBackInStockSubscriptionSearchService backInStockSubscriptionSearchService,
        ISearchPhraseParser phraseParser)
    {
        _backInStockSubscriptionSearchService = backInStockSubscriptionSearchService;
        _phraseParser = phraseParser;
    }

    public virtual async Task<BackInStockSubscriptionSearchResult> Handle(BackInStockSubscriptionsQuery request,
        CancellationToken cancellationToken)
    {
        var criteria = GetSearchCriteria(request);
        var result = await _backInStockSubscriptionSearchService.SearchAsync(criteria);

        return result;
    }

    protected virtual BackInStockSubscriptionSearchCriteria GetSearchCriteria(BackInStockSubscriptionsQuery request)
    {
        var criteria = request.GetSearchCriteria<BackInStockSubscriptionSearchCriteria>();
        criteria.StoreId = request.StoreId;
        criteria.UserId = request.UserId;
        criteria.ProductId = request.ProductId;
        criteria.IsActive = request.IsActive;

        if (!string.IsNullOrEmpty(request.Filter))
        {
            var parseResult = _phraseParser.Parse(request.Filter);

            criteria.Keyword = parseResult.Keyword;

            // Term filters
            foreach (var term in parseResult.Filters.OfType<TermFilter>())
            {
                term.MapTo(criteria);
            }

            // Custom ModifiedDate filter
            parseResult.Filters.Get<RangeFilter>("Triggered")
                .MapTo(x => criteria.StartTriggeredDate = x, x => criteria.EndTriggeredDate = x);
        }

        return criteria;
    }
}
