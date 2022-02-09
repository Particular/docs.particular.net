namespace EndpointB
{
    using Contracts;
    using NServiceBus;

    public class CommandHandler : // can handle both versions
        IHandleMessages<EndpointBCommand>,
        IHandleMessages<EndpointBCommandV2>
    {
        public Task Handle(EndpointBCommand message, IMessageHandlerContext context)
        {
            return HandleMessage(context, message.CorrelationId);
        }

        public Task Handle(EndpointBCommandV2 message, IMessageHandlerContext context)
        {
            return HandleMessage(context, message.CorrelationId, message.SomeOtherProperty);
        }

        Task HandleMessage(IMessageHandlerContext context, Guid id, string? optionalData = null)
        {
            Console.WriteLine("Received command - publish event now");
            return context.Publish<EndpointBEventV2>();
        }
    }
}