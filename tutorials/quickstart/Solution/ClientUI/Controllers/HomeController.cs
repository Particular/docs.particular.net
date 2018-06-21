using System.Web.Mvc;

namespace ClientUI.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Messages;
    using NServiceBus;

    public class HomeController : Controller
    {
        IEndpointInstance _endpointInstance;

        public HomeController(IEndpointInstance endpointInstance)
        {
            _endpointInstance = endpointInstance;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> PlaceOrder()
        {
            var command = new PlaceOrder
            {
                OrderId = Guid.NewGuid().ToString()
            };

            // Send the command
            await _endpointInstance.Send(command)
                .ConfigureAwait(false);

            return View();
        }
    }
}