using System.Threading.Tasks;
using System.Web.Mvc;

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
        Task<Status> statusTask = MvcApplication.Bus.Send("Samples.Callbacks.Receiver", message)
            .Register<Status>();

        return View("SendEnumMessage", await statusTask);
    }

    #endregion

    #region Web_SendIntMessage

    public async Task<ActionResult> SendIntMessage()
    {
        IntMessage message = new IntMessage();
        Task<int> responseTask = MvcApplication.Bus.Send("Samples.Callbacks.Receiver", message)
            .Register();

        return View("SendIntMessage", await responseTask);
    }

    #endregion

    #region Web_SendObjectMessage

    public async Task<ActionResult> SendObjectMessage()
    {
        ObjectMessage message = new ObjectMessage();
        Task<ObjectResponseMessage> responseTask = MvcApplication.Bus.Send("Samples.Callbacks.Receiver", message)
            .Register(completion => (ObjectResponseMessage) completion.Messages[0]);

        return View("SendObjectMessage", await responseTask);
    }

    #endregion
}