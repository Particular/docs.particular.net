using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

public class ReportVotesHandler : IHandleMessages<ReportVotes>
{
    private readonly ILogger<ReportVotesHandler> logger;

    public ReportVotesHandler(ILogger<ReportVotesHandler> logger)
    {
        this.logger = logger;
    }

    public Task Handle(ReportVotes message, IMessageHandlerContext context)
    {
        logger.LogInformation("Closing voting, {Candidate} got {NumberOfVotes} votes", message.Candidate, message.NumberOfVotes);
        return Task.FromResult(true);
    }
}