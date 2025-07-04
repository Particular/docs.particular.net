using System.Threading.Tasks;
using NServiceBus;

public class ReportZipCodeHandler :
    IHandleMessages<ReportZipCode>
{
    public Task Handle(ReportZipCode message, IMessageHandlerContext context)
    {
        Logger.Log("Closing voting, {ZipCode} voted {NumberOfVotes} times", message.ZipCode, message.NumberOfVotes);

        return Task.FromResult(true);
    }
}