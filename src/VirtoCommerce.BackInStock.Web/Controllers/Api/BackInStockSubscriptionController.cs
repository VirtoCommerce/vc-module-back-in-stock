using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.BackInStock.Core;
using VirtoCommerce.BackInStock.Core.Models;
using VirtoCommerce.BackInStock.Core.Services;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.BackInStock.Web.Controllers.Api;

[Route("api/back-in-stock/subscriptions")]
public class BackInStockSubscriptionController : Controller
{
    private readonly IBackInStockSubscriptionService _crudService;
    private readonly IBackInStockSubscriptionSearchService _searchService;

    public BackInStockSubscriptionController(
        IBackInStockSubscriptionService crudService,
        IBackInStockSubscriptionSearchService searchService)
    {
        _crudService = crudService;
        _searchService = searchService;
    }

    [HttpPost("search")]
    [Authorize(ModuleConstants.Security.Permissions.Read)]
    public async Task<ActionResult<BackInStockSubscriptionSearchResult>> Search([FromBody] BackInStockSubscriptionSearchCriteria criteria)
    {
        var result = await _searchService.SearchNoCloneAsync(criteria);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(ModuleConstants.Security.Permissions.Create)]
    public Task<ActionResult<BackInStockSubscription>> Create([FromBody] BackInStockSubscription model)
    {
        model.Id = null;
        return Update(model);
    }

    [HttpPut]
    [Authorize(ModuleConstants.Security.Permissions.Update)]
    public async Task<ActionResult<BackInStockSubscription>> Update([FromBody] BackInStockSubscription model)
    {
        await _crudService.SaveChangesAsync([model]);
        return Ok(model);
    }

    [HttpGet("{id}")]
    [Authorize(ModuleConstants.Security.Permissions.Read)]
    public async Task<ActionResult<BackInStockSubscription>> Get([FromRoute] string id, [FromQuery] string responseGroup = null)
    {
        var model = await _crudService.GetNoCloneAsync(id, responseGroup);
        return Ok(model);
    }

    [HttpDelete]
    [Authorize(ModuleConstants.Security.Permissions.Delete)]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete([FromQuery] string[] ids)
    {
        await _crudService.DeleteAsync(ids);
        return NoContent();
    }
}
