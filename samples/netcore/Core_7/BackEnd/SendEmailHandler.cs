using System.Threading.Tasks;
using Messages;
using NServiceBus;

public class SendEmailHandler : IHandleMessages<SendEmail>
{
    private readonly IEmailService emailService;

    public SendEmailHandler(IEmailService emailService)
    {
        this.emailService = emailService;
    }

    public Task Handle(SendEmail message, IMessageHandlerContext context)
    {
        emailService.SendEmail(message.CustomerName);

        return Task.CompletedTask;
    }
}