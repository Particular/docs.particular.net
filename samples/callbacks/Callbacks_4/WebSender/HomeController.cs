using Microsoft.AspNetCore.Mvc;
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
        var status = await messageSession.Request<Status>(message, sendOptions)
            .ConfigureAwait(false);
        return View("SendEnumMessage", status);
    }

    #endregion

    #region Web_SendIntMessage

    public async Task<ActionResult> SendIntMessage()
    {
        var message = new IntMessage();
        var sendOptions = new SendOptions();
        sendOptions.SetDestination("Samples.Callbacks.Receiver");
        var status = await messageSession.Request<int>(message, sendOptions)
            .ConfigureAwait(false);
        return View("SendIntMessage", status);
    }

    #endregion

    #region Web_SendObjectMessage

    public async Task<ActionResult> SendObjectMessage()
    {
        var message = new ObjectMessage();
        var sendOptions = new SendOptions();
        sendOptions.SetDestination("Samples.Callbacks.Receiver");
        var status = await messageSession.Request<ObjectResponseMessage>(message, sendOptions)
            .ConfigureAwait(false);
        return View("SendObjectMessage", status);
    }

    #endregion
}