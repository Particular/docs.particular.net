using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NServiceBus;

#region razor-page-usage
public class RazorPage(IMessageSession messageSession) : PageModel
{
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        await messageSession.Send(new MessageFromRazorPage());
        return RedirectToPage("./Success");
    }
}
#endregion

class MessageFromRazorPage
{
}