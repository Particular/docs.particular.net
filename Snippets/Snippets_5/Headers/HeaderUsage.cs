using System;
using NServiceBus;
using NServiceBus.MessageMutator;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;
using NServiceBus.Unicast.Messages;

class HeaderUsage
{
    #region header-incoming-behaviour
    public class SampleIncomingBehavior :
        IBehavior<IncomingContext>
    {
        public void Invoke(IncomingContext context, Action next)
        {
            var headers = context.PhysicalMessage.Headers;
            var nsbVersion = headers[Headers.NServiceBusVersion];
            var customHeader = headers["MyCustomHeader"];
            next();
        }
    }
    #endregion
    #region header-outgoing-behavior
    public class SampleOutgoingBehavior : IBehavior<OutgoingContext>
    {
        public void Invoke(OutgoingContext context, Action next)
        {
            var headers = context.OutgoingMessage.Headers;
            headers["MyCustomHeader"] = "My custom value";
            next();
        }
    }
    #endregion
    #region header-incoming-mutator
    public class SampleIncomingMutator : IMutateIncomingTransportMessages
    {
        public void MutateIncoming(TransportMessage transportMessage)
        {
            var headers = transportMessage.Headers;
            var nsbVersion = headers[Headers.NServiceBusVersion];
            var customHeader = headers["MyCustomHeader"];
        }
    }
    #endregion
    #region header-outgoing-mutator
    public class SampleOutgoingMutator : IMutateOutgoingTransportMessages
    {
        public void MutateOutgoing(LogicalMessage logicalMessage, TransportMessage transportMessage)
        {
            var headers = transportMessage.Headers;
            headers["MyCustomHeader"] = "My custom value";
        }
    }
    #endregion
    #region header-incoming-handler
    public class SampleReadHandler : IHandleMessages<MyMessage>
    {
        public IBus Bus { get; set; }

        public void Handle(MyMessage message)
        {
            var headers = Bus.CurrentMessageContext.Headers;
            var nsbVersion = headers[Headers.NServiceBusVersion];
            var customHeader = headers["MyCustomHeader"];
        }
    }
    #endregion
    #region header-outgoing-handler
    public class SampleWriteHandler : IHandleMessages<MyMessage>
    {
        public IBus Bus { get; set; }

        public void Handle(MyMessage message)
        {
            var headers = Bus.OutgoingHeaders;
            headers["MyCustomHeader"] = "My custom value";
        }
    }
    #endregion
    internal class MyMessage
    {
    }

}
