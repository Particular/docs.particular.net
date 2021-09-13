using System.Threading.Tasks;
using Messages;
using NServiceBus;

#region back-end-handler
public class SomeHandler : IHandleMessages<SomeMessage>
{
    private readonly ICalculateStuff stuffCalculator;

    public SomeHandler(ICalculateStuff stuffCalculator)
    {
        this.stuffCalculator = stuffCalculator;
    }

    public async Task Handle(SomeMessage message, IMessageHandlerContext context)
    {
        await stuffCalculator.Calculate(message.Number);
        
        // Do some more stuff if needed
    }
}
#endregion