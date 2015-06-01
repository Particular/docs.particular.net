using System;
using System.Collections.Generic;
using NServiceBus;
using NServiceBus.MessageMutator;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;

class HeaderUsage
{
    #region header-incoming-behavior
    public class SampleIncomingBehavior : PhysicalMessageProcessingStageBehavior
    {
        public override void Invoke(Context context, Action next)
        {
            Dictionary<string, string> headers = context.PhysicalMessage.Headers;
            string nsbVersion = headers[Headers.NServiceBusVersion];
            string customHeader = headers["MyCustomHeader"];
            next();
        }
    }
    #endregion
    #region header-outgoing-behavior
    public class SampleOutgoingBehavior : Behavior<OutgoingContext>
    {
        public override void Invoke(OutgoingContext context, Action next)
        {
            context.SetHeader("MyCustomHeader", "My custom value");
            next();
        }
    }
    #endregion
    #region header-incoming-mutator
    public class SampleIncomingMutator : IMutateIncomingTransportMessages
    {
        public void MutateIncoming(TransportMessage transportMessage)
        {
            Dictionary<string, string> headers = transportMessage.Headers;
            string nsbVersion = headers[Headers.NServiceBusVersion];
            string customHeader = headers["MyCustomHeader"];
        }
    }
    #endregion
    #region header-outgoing-mutator
    public class SampleOutgoingMutator : IMutateOutgoingPhysicalContext
    {
        public void MutateOutgoing(OutgoingPhysicalMutatorContext context)
        {
            context.SetHeader("MyCustomHeader", "My custom value");
        }
    }
    #endregion
    #region header-incoming-handler
    public class SampleReadHandler : IHandleMessages<MyMessage>
    {
        IBus bus;

        public SampleReadHandler(IBus bus)
        {
            this.bus = bus;
        }

        public void Handle(MyMessage message)
        {
            IDictionary<string, string> headers = bus.CurrentMessageContext.Headers;
            string nsbVersion = headers[Headers.NServiceBusVersion];
            string customHeader = headers["MyCustomHeader"];
        }
    }
    #endregion
    #region header-outgoing-handler
    public class SampleWriteHandler : IHandleMessages<MyMessage>
    {
        IBus bus;

        public SampleWriteHandler(IBus bus)
        {
            this.bus = bus;
        }

        public void Handle(MyMessage message)
        {
            SendOptions sendOptions = new SendOptions();

            sendOptions.SetHeader("MyCustomHeader", "My custom value");
            bus.Send(new SomeOtherMessage(), sendOptions);

            ReplyOptions replyOptions = new ReplyOptions();

            replyOptions.SetHeader("MyCustomHeader", "My custom value");
            bus.Reply(new SomeOtherMessage(), replyOptions);

            PublishOptions publishOptions = new PublishOptions();

            publishOptions.SetHeader("MyCustomHeader", "My custom value");
            bus.Publish(new SomeOtherMessage(), publishOptions);
        }
    }
    #endregion
    class SomeOtherMessage
    {
    }
    internal class MyMessage
    {
    }

    class StaticHeaders
    {
        public StaticHeaders()
        {

            #region header-static-endpoint
            BusConfiguration configuration = new BusConfiguration();
            configuration.AddHeaderToAllOutgoingMessages("MyGlobalHeader", "some static value");
            #endregion
        }
    }


}
