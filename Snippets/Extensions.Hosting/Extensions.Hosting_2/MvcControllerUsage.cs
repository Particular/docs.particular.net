using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;

public class MvcControllerUsage
{
    #region mvc-controller-usage
    public class MvcController : Controller
    {
        IMessageSession messageSession;

        public MvcController(IMessageSession messageSession)
        {
            this.messageSession = messageSession;
        }

        public ViewResult Index()
        {
            return View();
        }

        public async Task<ViewResult> SendMessage()
        {
            await messageSession.Send(new MessageFromMvc());
            return View();
        }
    }
    #endregion

    class MessageFromMvc
    {
    }
}