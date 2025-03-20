using NServiceBus;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

#region MyHandler
public class MyHandler :
    IHandleMessages<MyMessage>
{
    private readonly ILogger<MyHandler> logger;

    public MyHandler(ILogger<MyHandler> logger)
    {
        logger.LogInformation("Hello World! Logging is {Description}.", "fun");
        this.logger = logger;
    }

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        logger.LogInformation($"Message received. Id: {message.Id}");
        //throw new ArgumentNullException("Uh oh - something went wrong....");
        //throw new DivideByZeroException("DivideByZeroException - something went wrong....");
        return Task.CompletedTask;
    }
}
#endregion
