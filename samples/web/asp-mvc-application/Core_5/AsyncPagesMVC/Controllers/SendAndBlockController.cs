using System;
using System.Web.Mvc;
using NServiceBus;

public class SendAndBlockController :
    Controller
{
    IBus bus;

    public SendAndBlockController(IBus bus)
    {
        this.bus = bus;
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
            return View();

        #region SendAndBlockController

        var command = new Command
        {
            Id = number
        };

        var asyncResult = bus.Send("Samples.Mvc.Server", command)
            .Register(SimpleCommandCallback, this);
        var asyncWaitHandle = asyncResult.AsyncWaitHandle;
        asyncWaitHandle.WaitOne(50000);

        #endregion

        return View();
    }

    void SimpleCommandCallback(IAsyncResult asyncResult)
    {
        var result = (CompletionResult) asyncResult.AsyncState;
        var controller = (SendAndBlockController) result.State;
        controller.ViewBag.ResponseText = Enum.GetName(typeof(ErrorCodes), result.ErrorCode);
    }
}