using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

#region MessageSessionInjectionMvc
public class SampleController(IMessageSession messageSession) : Controller
#endregion
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult SendMessageMvc()
    {
        ViewBag.Title = "Send a message using Mvc";
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SendMessageMvc(string textField)
    {
        if (!int.TryParse(textField, out var number))
        {
            return View();
        }
        #region MVCSendMessage
        var command = new Command
        {
            Id = number
        };

        var status = await messageSession.Request<ErrorCodes>(command);

        ViewBag.Title = "Send a message using Mvc";
        ViewBag.ResponseText = Enum.GetName(typeof(ErrorCodes), status);
        return View();

        #endregion
    }

    [HttpGet]
    public IActionResult SendMessageBlazor()
    {
        ViewBag.Title = "Send a message using Blazor";
        return View();
    }
}