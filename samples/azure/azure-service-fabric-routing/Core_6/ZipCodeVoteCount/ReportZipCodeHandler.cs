using System.Threading.Tasks;
using NServiceBus;

public class ReportZipCodeHandler :
    IHandleMessages<ReportZipCode>
{
    public Task Handle(ReportZipCode message, IMessageHandlerContext context)
    {
        Logger.Log($"Closing voting, {message.ZipCode} voted {message.NumberOfVotes} times ");

        return Task.FromResult(true);
    }
}