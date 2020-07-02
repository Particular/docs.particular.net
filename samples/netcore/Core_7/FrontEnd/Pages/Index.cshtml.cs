using System.Threading.Tasks;
using Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NServiceBus;

namespace FrontEnd.Pages
{
    public class IndexModel : PageModel
    {
        IMessageSession messageSession;

        public IndexModel(IMessageSession messageSession)
        {
            this.messageSession = messageSession;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Customer Customer { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }

            await messageSession.Send(new SendEmail { CustomerName = Customer.Name })
                .ConfigureAwait(false);

            return RedirectToPage();
        }
    }
}