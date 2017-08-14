using System;
using System.Web.Mvc;
using NServiceBus;

public class SendAsyncController :
    AsyncController
{
    IBus bus;

    public SendAsyncController(IBus bus)
    {
        this.bus = bus;
    }

    [HttpGet]
    public ActionResult Index()
    {
        ViewBag.Title = "SendAsync";
        return View("Index");
    }

    [HttpPost]
    [AsyncTimeout(50000)]
    public void IndexAsync(string textField)
    {
        if (!int.TryParse(textField, out var number))
        {
            return;
        }

        #region AsyncController

        var command = new Command
        {
            Id = number
        };
        bus.Send("Samples.Mvc.Server", command)
            .Register<int>(status =>
            {
                AsyncManager.Parameters["errorCode"] = Enum.GetName(typeof(ErrorCodes), status);
            });

        #endregion
    }

    public ActionResult IndexCompleted(string errorCode)
    {
        ViewBag.Title = "SendAsync";
        ViewBag.ResponseText = errorCode;
        return View("Index");
    }
}