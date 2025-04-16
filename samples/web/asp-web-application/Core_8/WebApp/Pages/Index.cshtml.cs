using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NServiceBus;

namespace WebApp.Pages;

[IgnoreAntiforgeryToken]
public class IndexModel(IMessageSession messageSession) : PageModel
{
    public string ResponseText { get; set; }

    public async Task<IActionResult> OnPostAsync(string textField)
    {
        if (string.IsNullOrWhiteSpace(textField))
        {
            return Page();
        }

        #region ActionHandling

        if (!int.TryParse(textField, out var number))
        {
            return Page();
        }
        var command = new Command
        {
            Id = number
        };

        var sendOptions = new SendOptions();
        sendOptions.SetDestination("Samples.AsyncPages.Server");

        var code = await messageSession.Request<ErrorCodes>(command, sendOptions);
        ResponseText = Enum.GetName(typeof(ErrorCodes), code);

        return Page();
        #endregion
    }
}