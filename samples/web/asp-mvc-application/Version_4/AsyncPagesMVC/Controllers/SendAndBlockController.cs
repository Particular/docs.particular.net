using System;
using System.Web.Mvc;
using NServiceBus;
using System.Threading;

public class SendAndBlockController : Controller
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

        int number;
        if (!int.TryParse(textField, out number))
            return View();
        #region SendAndBlockController
        Command command = new Command { Id = number };

        IAsyncResult res =  bus.Send("Samples.Mvc.Server", command)
            .Register(SimpleCommandCallback, this);
        WaitHandle asyncWaitHandle = res.AsyncWaitHandle;
        asyncWaitHandle.WaitOne(50000);
        #endregion
        return View();
    }
        
    void SimpleCommandCallback(IAsyncResult asyncResult)
    {
        CompletionResult result = (CompletionResult)asyncResult.AsyncState;
        SendAndBlockController controller = (SendAndBlockController)result.State;
        controller.ViewBag.ResponseText = Enum.GetName(typeof (ErrorCodes), result.ErrorCode);
    }

}
