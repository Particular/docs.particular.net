using Microsoft.AspNetCore.Mvc;
using NServiceBus;

namespace WebApp.Controllers;

public class SampleController(IMessageSession messageSession) : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult SendMessageMvc()
    {
        return View();
    }

    #region MVCSendMessage
    [HttpPost]
    public async Task<IActionResult> SendMessageMvc(string textField)
    {
        if (!int.TryParse(textField, out var number))
        {
            return View();
        }
        var command = new Command
        {
            Id = number
        };

        await messageSession.Send(command);
        ViewBag.ResponseText = $"Sent message with Id {command.Id}";

        return View();
    }
    #endregion

    [HttpGet]
    public IActionResult SendMessageBlazor()
    {
        return View();
    }
}