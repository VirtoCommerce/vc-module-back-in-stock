using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.BackInStock.Core;

namespace VirtoCommerce.BackInStock.Web.Controllers.Api
{
    [Route("api/back-in-stock")]
    public class BackInStockController : Controller
    {
        // GET: api/back-in-stock
        /// <summary>
        /// Get message
        /// </summary>
        /// <remarks>Return "Hello world!" message</remarks>
        [HttpGet]
        [Route("")]
        [Authorize(ModuleConstants.Security.Permissions.Read)]
        public ActionResult<string> Get()
        {
            return Ok(new { result = "Hello world!" });
        }
    }
}
