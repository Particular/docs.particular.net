using Microsoft.AspNetCore.Mvc;

public class SampleController(IMessageSession messageSession) :
    Controller
{
    public ActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public ActionResult SendAndBlock()
    {
        ViewBag.Title = "SendAndBlock";
        return View("SendMessage");
    }

    [HttpPost]
    public ActionResult SendAndBlock(string textField)
    {
        ViewBag.Title = "SendAndBlock";

        if (!int.TryParse(textField, out var number))
        {
            return View("SendMessage");
        }

        #region SendAndBlockController

        var command = new Command
        {
            Id = number
        };

        var status = messageSession.Request<ErrorCodes>(command).GetAwaiter().GetResult();

        ViewBag.Title = "SendAndBlock";
        ViewBag.ResponseText = Enum.GetName(typeof(ErrorCodes), status);
        return View("SendMessage");

        #endregion
    }

    [HttpGet]
    public ActionResult SendAsync()
    {
        ViewBag.Title = "SendAsync";
        return View("SendMessage");
    }

    [HttpPost]
    public async Task<ActionResult> SendAsync(string textField)
    {
        if (!int.TryParse(textField, out var number))
        {
            return View("SendMessage");
        }
        #region AsyncController
        var command = new Command
        {
            Id = number
        };

        var status = await messageSession.Request<ErrorCodes>(command);

        ViewBag.Title = "SendAndBlock";
        ViewBag.ResponseText = Enum.GetName(typeof(ErrorCodes), status);
        return View("SendMessage");

        #endregion
    }
}
