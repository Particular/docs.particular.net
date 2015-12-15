using System.Threading.Tasks;
using System.Web.Mvc;
using NServiceBus;

public class HomeController : Controller
{
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
        Task<Status> statusTask = MvcApplication.BusSession.Request<Status>(message, sendOptions);
        return View("SendEnumMessage", await statusTask);
    }

    #endregion

    #region Web_SendIntMessage

    public async Task<ActionResult> SendIntMessage()
    {
        IntMessage message = new IntMessage();
        SendOptions sendOptions = new SendOptions();
        sendOptions.SetDestination("Samples.Callbacks.Receiver");
        Task<int> statusTask = MvcApplication.BusSession.Request<int>(message, sendOptions);
        return View("SendIntMessage", await statusTask);
    }

    #endregion

    #region Web_SendObjectMessage

    public async Task<ActionResult> SendObjectMessage()
    {
        ObjectMessage message = new ObjectMessage();
        SendOptions sendOptions = new SendOptions();
        sendOptions.SetDestination("Samples.Callbacks.Receiver");
        Task<ObjectResponseMessage> responseTask = MvcApplication.BusSession.Request<ObjectResponseMessage>(message, sendOptions);
        return View("SendObjectMessage", await responseTask);
    }

    #endregion
}