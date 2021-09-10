using System.Threading.Tasks;
using FrontEnd.Models;
using Messages;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;

namespace FrontEnd
{
    #region front-end-controller
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
        public async Task<ActionResult> Index(ViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            await messageSession.Send(new SomeMessage { Number = model.Number })
                .ConfigureAwait(false);

            ViewBag.Message = "Request was queued successfully.";
            
            return View(model);
        }
    }
    #endregion
}