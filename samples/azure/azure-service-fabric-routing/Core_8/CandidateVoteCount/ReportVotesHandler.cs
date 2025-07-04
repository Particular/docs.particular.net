using System.Threading.Tasks;
using NServiceBus;

public class ReportVotesHandler :
    IHandleMessages<ReportVotes>
{
    public Task Handle(ReportVotes message, IMessageHandlerContext context)
    {
        Logger.Log("Closing voting, {Candidate} got {NumberOfVotes} votes", message.Candidate, message.NumberOfVotes);

        return Task.FromResult(true);
    }
}