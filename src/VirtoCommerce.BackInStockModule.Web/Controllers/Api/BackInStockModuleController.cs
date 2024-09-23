using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.BackInStockModule.Core;

namespace VirtoCommerce.BackInStockModule.Web.Controllers.Api
{
    [Route("api/back-in-stock-module")]
    public class BackInStockModuleController : Controller
    {
        // GET: api/back-in-stock-module
        /// <summary>
        /// Get message
        /// </summary>
        /// <remarks>Return "Hello world!" message</remarks>
        [HttpGet]
        [Route("")]
        [Authorize(ModuleConstants.Security.Permissions.BackInStockSubscriptionRead)]
        public ActionResult<string> Get()
        {
            return Ok(new { result = "Hello world!" });
        }
    }
}
