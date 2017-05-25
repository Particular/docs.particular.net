using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using NServiceBus;

public class SendAsyncController :
    AsyncController
{
    IEndpointInstance endpoint;

    public SendAsyncController(IEndpointInstance endpoint)
    {
        this.endpoint = endpoint;
    }

    [HttpGet]
    public ActionResult Index()
    {
        ViewBag.Title = "SendAsync";
        return View("Index");
    }

    [HttpPost]
    [AsyncTimeout(50000)]
    public async Task<ActionResult> IndexAsync(string textField)
    {
        int number;
        if (!int.TryParse(textField, out number))
        {
            return View("Index");
        }
        #region AsyncController
        var command = new Command
                        {
                            Id = number
                        };

        var sendOptions = new SendOptions();
        sendOptions.SetDestination("Samples.Mvc.Server");

        var status = await endpoint.Request<ErrorCodes>(command, sendOptions)
            .ConfigureAwait(false);

        return IndexCompleted(Enum.GetName(typeof(ErrorCodes), status));

        #endregion
    }

    public ActionResult IndexCompleted(string errorCode)
    {
        ViewBag.Title = "SendAsync";
        ViewBag.ResponseText = errorCode;
        return View("Index");
    }
}