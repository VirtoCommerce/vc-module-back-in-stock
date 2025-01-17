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
public class BackInStockSubscriptionController(
    IBackInStockSubscriptionService crudService,
    IBackInStockSubscriptionSearchService searchService)
    : Controller
{
    [HttpPost("search")]
    [Authorize(ModuleConstants.Security.Permissions.Read)]
    public async Task<ActionResult<BackInStockSubscriptionSearchResult>> Search([FromBody] BackInStockSubscriptionSearchCriteria criteria)
    {
        var result = await searchService.SearchNoCloneAsync(criteria);
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
        await crudService.SaveChangesAsync([model]);
        return Ok(model);
    }

    [HttpGet("{id}")]
    [Authorize(ModuleConstants.Security.Permissions.Read)]
    public async Task<ActionResult<BackInStockSubscription>> Get([FromRoute] string id, [FromQuery] string responseGroup = null)
    {
        var model = await crudService.GetNoCloneAsync(id, responseGroup);
        return Ok(model);
    }

    [HttpDelete]
    [Authorize(ModuleConstants.Security.Permissions.Delete)]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete([FromQuery] string[] ids)
    {
        await crudService.DeleteAsync(ids);
        return NoContent();
    }
}
