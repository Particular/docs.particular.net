using System;
using System.Threading.Tasks;
using NServiceBus;
using ServiceControl.Contracts;

namespace EndpointsMonitor
{
    public class CustomEventsHandler : IHandleMessages<MessageFailed>
    {
        public Task Handle(MessageFailed message, IMessageHandlerContext context)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("Received ServiceControl 'MessageFailed' event for a SimpleMessage.");
            
            return Task.FromResult(0);
        }
    }
}
