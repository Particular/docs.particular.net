using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NServiceBus;

namespace WebApplication.Core.Pages
{
    [IgnoreAntiforgeryToken(Order = 1001)]
    public class IndexModel : PageModel
    {
        public string ResponseText { get; set; }

        private IEndpointInstance endpoint;
        public IndexModel(IEndpointInstance endpoint)
        {
            this.endpoint = endpoint;
        }
        public void OnGet()
        {

        }

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

            var code = await endpoint.Request<ErrorCodes>(command, sendOptions);
            ResponseText = Enum.GetName(typeof(ErrorCodes), code);

            return Page();
            #endregion
        }
    }
}
