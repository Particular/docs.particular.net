using System;
using NServiceBus;
using NServiceBus.MessageMutator;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;

class HeaderUsage
{
    #region header-incoming-behavior
    public class SampleIncomingBehavior : IBehavior<ReceivePhysicalMessageContext>
    {
        public void Invoke(ReceivePhysicalMessageContext context, Action next)
        {
            var headers = context.PhysicalMessage.Headers;
            var nsbVersion = headers[Headers.NServiceBusVersion];
            var customHeader = headers["MyCustomHeader"];
            next();
        }
    }
    #endregion
    #region header-outgoing-in-behavior
    public class SampleOutgoingBehavior : IBehavior<SendPhysicalMessageContext>
    {
        public void Invoke(SendPhysicalMessageContext context, Action next)
        {
            var headers = context.MessageToSend.Headers;
            headers["MyCustomHeader"] = "My custom value";
            next();
        }
    }
    #endregion
    #region header-incoming-in-mutator
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
    #region header-outgoing-in-mutator
    public class SampleOutgoingMutator : IMutateOutgoingTransportMessages
    {
        public void MutateOutgoing(object[] messages, TransportMessage transportMessage)
        {
            var headers = transportMessage.Headers;
            headers["MyCustomHeader"] = "My custom value";
        }
    }
    #endregion
    #region header-incoming-in-handler
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
    #region header-outgoing-in-handler
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
