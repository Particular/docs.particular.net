#pragma warning disable 1998
namespace Core.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using NServiceBus;

    #region abac-pep-as-nservicebus-handler

    public class ProcessOrderHandler : IHandleMessages<ProcessOrder>
    {
        private readonly IAuthorizationService authService;

        public ProcessOrderHandler(IAuthorizationService authService)
        {
            this.authService = authService;
        }

        public async Task Handle(ProcessOrder message, IMessageHandlerContext context)
        {
            // PEP: Intercept and check authorization before processing
            var attributes = new Dictionary<string, object>
            {
                ["OrderAmount"] = message.Amount,
                ["UserId"] = message.UserId,
                ["Region"] = message.Region
            };

            if (!await authService.AuthorizeAsync(attributes))
            {
                throw new UnauthorizedAccessException("Order processing denied");
            }

            // Business logic continues only if authorized

            await Task.CompletedTask;
        }
    }

    #endregion

    public interface IAuthorizationService
    {
        Task<bool> AuthorizeAsync(Dictionary<string, object> attributes);
    }

    public class ProcessOrder : ICommand
    {
        public decimal Amount { get; set; }
        public string UserId { get; set; }
        public string Region { get; set; }
    }
}