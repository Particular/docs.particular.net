using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NServiceBus;
using System.Threading.Tasks;

public class HomeController : Controller
{
    private readonly IMessageSession messageSession;

    public HomeController(IMessageSession messageSession)
    {
        this.messageSession = messageSession;
    }

    public ActionResult Index()
    {
        return View();
    }

    #region Web_SendEnumMessage

    public async Task<ActionResult> SendEnumMessage()
    {
        var message = new EnumMessage();
        var sendOptions = new SendOptions();
        sendOptions.SetDestination("Samples.Callbacks.Receiver");
        var statusTask = messageSession.Request<Status>(message, sendOptions);
        return View("SendEnumMessage", await statusTask.ConfigureAwait(false));
    }

    #endregion

    #region Web_SendIntMessage

    public async Task<ActionResult> SendIntMessage()
    {
        var message = new IntMessage();
        var sendOptions = new SendOptions();
        sendOptions.SetDestination("Samples.Callbacks.Receiver");
        var statusTask = messageSession.Request<int>(message, sendOptions);
        return View("SendIntMessage", await statusTask.ConfigureAwait(false));
    }

    #endregion

    #region Web_SendObjectMessage

    public async Task<ActionResult> SendObjectMessage()
    {
        var message = new ObjectMessage();
        var sendOptions = new SendOptions();
        sendOptions.SetDestination("Samples.Callbacks.Receiver");
        var statusTask = messageSession.Request<ObjectResponseMessage>(message, sendOptions);
        return View("SendObjectMessage", await statusTask.ConfigureAwait(false));
    }

    #endregion
}