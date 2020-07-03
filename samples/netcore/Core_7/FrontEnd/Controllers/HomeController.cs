using System.Threading.Tasks;
using FrontEnd.Models;
using Messages;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;

namespace FrontEnd
{
    public class HomeController : Controller
    {
        IMessageSession messageSession;
        
        public HomeController(IMessageSession messageSession)
        {
            this.messageSession = messageSession;
        }

        public ActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<ActionResult> Index(CustomerModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            await messageSession.Send(new SendEmail { CustomerName = model.Name })
                .ConfigureAwait(false);

            ViewBag.Message = "Email message queued.";
            
            return View(model);
        }
    }
}