using Messages;
using NServiceBus;
using NServiceBus.Logging;

public class ResponseHandler :
    IHandleMessages<Response>
{
    static ILog log = LogManager.GetLogger(typeof(ResponseHandler));

    public void Handle(Response message)
    {
        log.Info($"Response received from server for request with id:{message.ResponseId}" );
    }
}