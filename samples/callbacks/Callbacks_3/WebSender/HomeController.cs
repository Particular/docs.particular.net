using System.Threading.Tasks;
using System.Web.Mvc;
using NServiceBus;

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
        var sendOptions = new SendOptions();
        sendOptions.SetDestination("Samples.Callbacks.Receiver");
        var endpointInstance = MvcApplication.EndpointInstance;
        var statusTask = endpointInstance.Request<Status>(message, sendOptions);
        return View("SendEnumMessage", await statusTask.ConfigureAwait(false));
    }

    #endregion

    #region Web_SendIntMessage

    public async Task<ActionResult> SendIntMessage()
    {
        var message = new IntMessage();
        var sendOptions = new SendOptions();
        sendOptions.SetDestination("Samples.Callbacks.Receiver");
        var endpointInstance = MvcApplication.EndpointInstance;
        var statusTask = endpointInstance.Request<int>(message, sendOptions);
        return View("SendIntMessage", await statusTask.ConfigureAwait(false));
    }

    #endregion

    #region Web_SendObjectMessage

    public async Task<ActionResult> SendObjectMessage()
    {
        var message = new ObjectMessage();
        var sendOptions = new SendOptions();
        sendOptions.SetDestination("Samples.Callbacks.Receiver");
        var endpointInstance = MvcApplication.EndpointInstance;
        var responseTask = endpointInstance
            .Request<ObjectResponseMessage>(message, sendOptions);
        return View("SendObjectMessage", await responseTask.ConfigureAwait(false));
    }

    #endregion
}