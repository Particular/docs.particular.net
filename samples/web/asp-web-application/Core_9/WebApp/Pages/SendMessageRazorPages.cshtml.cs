using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    [IgnoreAntiforgeryToken]
    public class RazorPagesModel(IMessageSession messageSession) : PageModel
    {
        public string? ResponseText { get; set; }

        #region RazorPagesSendMessage
        public async Task<IActionResult> OnPostAsync(string textField)
        {
            if (string.IsNullOrWhiteSpace(textField))
            {
                return Page();
            }


            if (!int.TryParse(textField, out var number))
            {
                return Page();
            }
            var command = new Command
            {
                Id = number
            };

            await messageSession.Send(command);
            ResponseText = $"Sent message with Id {command.Id}";

            return Page();
        }
        #endregion
    }
}