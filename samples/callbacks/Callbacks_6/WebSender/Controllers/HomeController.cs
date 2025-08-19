using Microsoft.AspNetCore.Mvc;
using NServiceBus;

public class HomeController(IMessageSession messageSession) : Controller
{
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
        var status = await messageSession.Request<Status>(message, sendOptions);
        return View("SendEnumMessage", status);
    }

    #endregion

    #region Web_SendIntMessage

    public async Task<ActionResult> SendIntMessage()
    {
        var message = new IntMessage();
        var sendOptions = new SendOptions();
        sendOptions.SetDestination("Samples.Callbacks.Receiver");
        var status = await messageSession.Request<int>(message, sendOptions);
        return View("SendIntMessage", status);
    }

    #endregion

    #region Web_SendObjectMessage

    public async Task<ActionResult> SendObjectMessage()
    {
        var message = new ObjectMessage();
        var sendOptions = new SendOptions();
        sendOptions.SetDestination("Samples.Callbacks.Receiver");
        var status = await messageSession.Request<ObjectResponseMessage>(message, sendOptions);
        return View("SendObjectMessage", status);
    }

    #endregion
}