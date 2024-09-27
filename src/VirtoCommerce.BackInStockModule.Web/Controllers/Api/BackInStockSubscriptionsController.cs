using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.BackInStockModule.Core;
using VirtoCommerce.BackInStockModule.Core.Models;
using VirtoCommerce.BackInStockModule.Core.Services;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.BackInStockModule.Web.Controllers.Api;

[Route("api/back-in-stock/subscriptions")]
[ApiController]
public class BackInStockSubscriptionsController : Controller
{
    private readonly IBackInStockSubscriptionSearchService _searchService;
    private readonly IBackInStockSubscriptionService _crudService;

    public BackInStockSubscriptionsController(
        IBackInStockSubscriptionSearchService searchService,
        IBackInStockSubscriptionService crudService)
    {
        _searchService = searchService;
        _crudService = crudService;
    }

    /// <summary>
    /// Return customers back in stock subscriptions search results
    /// </summary>
    [HttpPost]
    [Route("search")]
    [Authorize(ModuleConstants.Security.Permissions.Read)]
    public async Task<ActionResult<BackInStockSubscriptionSearchResult>> Search([FromBody] BackInStockSubscriptionSearchCriteria criteria)
    {
        var result = await _searchService.SearchNoCloneAsync(criteria);
        return Ok(result);
    }

    /// <summary>
    ///  Create new or update existing back in stock subscription
    /// </summary>
    /// <param name="subscriptions">back in stock subscription</param>
    /// <returns></returns>
    [HttpPost]
    [Route("")]
    [Authorize(ModuleConstants.Security.Permissions.Update)]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Update([FromBody] BackInStockSubscription[] subscriptions)
    {
        await _crudService.SaveChangesAsync(subscriptions);
        return NoContent();
    }

    /// <summary>
    /// Delete back in stock subscription by IDs
    /// </summary>
    /// <param name="ids">IDs</param>
    /// <returns></returns>
    [HttpDelete]
    [Route("")]
    [Authorize(ModuleConstants.Security.Permissions.Delete)]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete([FromQuery] string[] ids)
    {
        await _crudService.DeleteAsync(ids);
        return NoContent();
    }
}
