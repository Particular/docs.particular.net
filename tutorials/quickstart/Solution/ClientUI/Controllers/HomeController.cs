using System.Dynamic;
using System.Threading;
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
        static int messagesSent;

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
            var orderId = Guid.NewGuid().ToString().Substring(0, 8);

            var command = new PlaceOrder { OrderId = orderId };

            // Send the command
            await _endpointInstance.Send(command)
                .ConfigureAwait(false);

            dynamic model = new ExpandoObject();
            model.OrderId = orderId;
            model.MessagesSent = Interlocked.Increment(ref messagesSent);

            return View(model);
        }
    }
}