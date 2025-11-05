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

            var code = await messageSession.Request<ErrorCodes>(command);
            ResponseText = Enum.GetName(typeof(ErrorCodes), code);

            return Page();
        }
        #endregion
    }
}