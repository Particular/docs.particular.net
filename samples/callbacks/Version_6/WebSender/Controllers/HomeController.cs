using System.Threading.Tasks;
using System.Web.Mvc;

namespace WebSender.Controllers
{
    using NServiceBus;

    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        #region Web_SendEnumMessage
        public async Task<ActionResult> SendEnumMessage()
        {
            EnumMessage message = new EnumMessage();

            SendOptions sendOptions = new SendOptions();
            sendOptions.SetDestination("Samples.Callbacks.Receiver");
            Status status = await ServiceBus.Bus.Request<Status>(message, sendOptions);

            return View("SendEnumMessage", status);
        }
        #endregion

        #region Web_SendIntMessage
        public async Task<ActionResult> SendIntMessage()
        {
            IntMessage message = new IntMessage();

            SendOptions sendOptions = new SendOptions();
            sendOptions.SetDestination("Samples.Callbacks.Receiver");
            int response = await ServiceBus.Bus.Request<int>(message, sendOptions);

            return View("SendIntMessage", response);
        }
        #endregion

        #region Web_SendObjectMessage
        public async Task<ActionResult> SendObjectMessage()
        {
            ObjectMessage message = new ObjectMessage();

            SendOptions sendOptions = new SendOptions();
            sendOptions.SetDestination("Samples.Callbacks.Receiver");
            ObjectResponseMessage response = await ServiceBus.Bus.Request<ObjectResponseMessage>(message, sendOptions);

            return View("SendObjectMessage", response);
        }
        #endregion
    }
}