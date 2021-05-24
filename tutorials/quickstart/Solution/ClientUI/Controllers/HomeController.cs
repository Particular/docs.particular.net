using System;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using Messages;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;

namespace ClientUI.Controllers
{
    [Route("[controller]")]
    public class HomeController : Controller
    {
        static int messagesSent;
        private readonly IMessageSession _messageSession;

        public HomeController(IMessageSession endpointInstance)
        {
            _messageSession = endpointInstance;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> PlaceOrder()
        {
            string orderId = Guid.NewGuid().ToString().Substring(0, 8);

            var command = new PlaceOrder { OrderId = orderId };

            // Send the command
           await _messageSession.Send(command)
               .ConfigureAwait(false);

            await Task.CompletedTask;

            dynamic model = new ExpandoObject();
            model.OrderId = orderId;
            model.MessagesSent = Interlocked.Increment(ref messagesSent);

            return View(model);
        }
    }
}
