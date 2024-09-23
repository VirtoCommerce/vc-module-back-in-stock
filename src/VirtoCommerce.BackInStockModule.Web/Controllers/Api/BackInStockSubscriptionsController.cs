using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.BackInStockModule.Core;
using VirtoCommerce.BackInStockModule.Core.Models;
using VirtoCommerce.BackInStockModule.Core.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.StoreModule.Core.Services;

namespace VirtoCommerce.BackInStockModule.Web.Controllers.Api
{
    [Route("api/backInStock")]
    [ApiController]
    public class BackInStockSubscriptionsController : Controller
    {
        private readonly IBackInStockSubscriptionSearchService _backInStockSubscriptionSearchService;
        private readonly IBackInStockSubscriptionService _backInStockSubscriptionService;
        private readonly IStoreService _storeService;

        public BackInStockSubscriptionsController(
            IBackInStockSubscriptionSearchService backInStockSubscriptionSearchService,
            IBackInStockSubscriptionService backInStockSubscriptionService,
            IStoreService storeService)
        {
            _backInStockSubscriptionSearchService = backInStockSubscriptionSearchService;
            _backInStockSubscriptionService = backInStockSubscriptionService;
            _storeService = storeService;
        }

        /// <summary>
        /// Return customers back in stock subscriptions search results
        /// </summary>
        [HttpPost]
        [Route("search")]
        [Authorize(ModuleConstants.Security.Permissions.BackInStockSubscriptionRead)]
        public async Task<ActionResult<BackInStockSubscriptionSearchResult>> SearchBackInStock(
            [FromBody] BackInStockSubscriptionSearchCriteria criteria)
        {
            var backInStockSubscriptionSearchResult =
                await _backInStockSubscriptionSearchService.SearchNoCloneAsync(criteria);
            return Ok(backInStockSubscriptionSearchResult);
        }

        /// <summary>
        ///  Create new or update existing customer review
        /// </summary>
        /// <param name="backInStock">Customer reviews</param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [Authorize(ModuleConstants.Security.Permissions.BackInStockSubscriptionUpdate)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Update([FromBody] BackInStockSubscription[] backInStock)
        {
            await _backInStockSubscriptionService.SaveChangesAsync(backInStock);
            return NoContent();
        }

        /// <summary>
        /// Delete Customer Reviews by IDs
        /// </summary>
        /// <param name="ids">IDs</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("")]
        [Authorize(ModuleConstants.Security.Permissions.BackInStockSubscriptionDelete)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Delete([FromQuery] string[] ids)
        {
            await _backInStockSubscriptionService.DeleteAsync(ids);
            return NoContent();
        }
    }
}
