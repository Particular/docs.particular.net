using System.Threading.Tasks;
using System.Web.Mvc;

public class HomeController :
    Controller
{
    public ActionResult Index()
    {
        return View();
    }

    #region Web_SendEnumMessage

    public async Task<ActionResult> SendEnumMessage()
    {
        var message = new EnumMessage();
        var statusTask = MvcApplication.Bus.Send("Samples.Callbacks.Receiver", message)
            .Register<Status>()
            .ConfigureAwait(false);
        return View("SendEnumMessage", await statusTask);
    }

    #endregion

    #region Web_SendIntMessage

    public async Task<ActionResult> SendIntMessage()
    {
        var message = new IntMessage();
        var responseTask = MvcApplication.Bus.Send("Samples.Callbacks.Receiver", message)
            .Register()
            .ConfigureAwait(false);
        return View("SendIntMessage", await responseTask);
    }

    #endregion

    #region Web_SendObjectMessage

    public async Task<ActionResult> SendObjectMessage()
    {
        var message = new ObjectMessage();
        var responseTask = MvcApplication.Bus.Send("Samples.Callbacks.Receiver", message)
            .Register(completion => (ObjectResponseMessage) completion.Messages[0])
            .ConfigureAwait(false);
        return View("SendObjectMessage", await responseTask);
    }

    #endregion
}
