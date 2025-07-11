using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

public class ReportZipCodeHandler :
    IHandleMessages<ReportZipCode>
{
    private readonly ILogger<ReportZipCodeHandler> logger;

    public ReportZipCodeHandler(ILogger<ReportZipCodeHandler> logger)
    {
        this.logger = logger;
    }

    public Task Handle(ReportZipCode message, IMessageHandlerContext context)
    {
        logger.LogInformation("Closing voting, {ZipCode} voted {NumberOfVotes} times", message.ZipCode, message.NumberOfVotes);

        return Task.FromResult(true);
    }
}