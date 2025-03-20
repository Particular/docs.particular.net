using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;
using RabbitMQ.Client.Events;

#region rabbitmq-access-to-event-args
class AccessToBasicDeliverEventArgs : Behavior<IIncomingContext>
{
    public override Task Invoke(IIncomingContext context, Func<Task> next)
    {
        var userIdOnBroker = context.Extensions.Get<BasicDeliverEventArgs>().BasicProperties.UserId;

        //do something useful

        return next();
    }
}
#endregion