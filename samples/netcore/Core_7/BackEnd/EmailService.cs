using Microsoft.Extensions.Logging;

internal class EmailService : IEmailService
{
    private readonly ILogger<EmailService> logger;

    public EmailService(ILogger<EmailService> logger)
    {
        this.logger = logger;
    }

    public void SendEmail(string name)
    {
        logger.LogInformation($"Sending email to {name}");
    }
}
