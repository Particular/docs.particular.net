using NServiceBus;
using Serilog;

public class MyHandler :
    IHandleMessages<MyMessage>
{
    static ILogger log = Log.ForContext<MyHandler>();

    public void Handle(MyMessage message)
    {
        log.Information("Hello from {@Handler}", nameof(MyHandler));
    }
}