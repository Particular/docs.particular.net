using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;

namespace WebApplication.Core.Controllers
{
    public class DefaultController : Controller
    {
        IEndpointInstance endpoint;

        public DefaultController(IEndpointInstance endpoint)
        {
            this.endpoint = endpoint;
        }

        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public async Task<ActionResult> Send()
        {
            await endpoint.Send("Samples.Mvc.Endpoint", new MyMessage())
                .ConfigureAwait(false);
            return RedirectToAction("Index", "Default");
        }

    }
}
