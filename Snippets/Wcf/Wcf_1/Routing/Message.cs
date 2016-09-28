namespace Wcf1.Routing
{
    using NServiceBus;

    public class Request :
        ICommand
    {
    }

    public class Response :
        IMessage
    {
    }
}