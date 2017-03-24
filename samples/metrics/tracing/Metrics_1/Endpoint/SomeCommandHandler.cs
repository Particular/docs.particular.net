using System.Threading.Tasks;
using NServiceBus;

class SomeCommandHandler : IHandleMessages<SomeCommand>
{
    public Task Handle(SomeCommand message, IMessageHandlerContext context) => Task.CompletedTask;
}