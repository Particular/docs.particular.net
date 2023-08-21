using System.Threading.Tasks;
using Messages;
using NServiceBus;

namespace Receiver;

public class CalculateFibonacciHandler : IHandleMessages<CalculateFibonacci>
{
    public Task Handle(CalculateFibonacci message, IMessageHandlerContext context)
    {
        return Task.CompletedTask;
    }
}