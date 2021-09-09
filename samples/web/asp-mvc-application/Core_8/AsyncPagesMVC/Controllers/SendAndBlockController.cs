using System;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;

public class SendAndBlockController :
    Controller
{
    IMessageSession messageSession;

    public SendAndBlockController(IMessageSession messageSession)
    {
        this.messageSession = messageSession;
    }

    [HttpGet]
    public ActionResult Index()
    {
        ViewBag.Title = "SendAndBlock";
        return View();
    }

    [HttpPost]
    public ActionResult Index(string textField)
    {
        ViewBag.Title = "SendAndBlock";

        if (!int.TryParse(textField, out var number))
        {
            return View();
        }

        #region SendAndBlockController

        var command = new Command
        {
            Id = number
        };


        var sendOptions = new SendOptions();
        sendOptions.SetDestination("Samples.Mvc.Server");
        var status = messageSession.Request<ErrorCodes>(command, sendOptions).GetAwaiter().GetResult();

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
