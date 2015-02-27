using System;
using System.Collections.Generic;
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
            Dictionary<string, string> headers = context.PhysicalMessage.Headers;
            string nsbVersion = headers[Headers.NServiceBusVersion];
            string customHeader = headers["MyCustomHeader"];
            next();
        }
    }
    #endregion
    #region header-outgoing-in-behavior
    public class SampleOutgoingBehavior : IBehavior<SendPhysicalMessageContext>
    {
        public void Invoke(SendPhysicalMessageContext context, Action next)
        {
            Dictionary<string, string> headers = context.MessageToSend.Headers;
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
            Dictionary<string, string> headers = transportMessage.Headers;
            string nsbVersion = headers[Headers.NServiceBusVersion];
            string customHeader = headers["MyCustomHeader"];
        }
    }
    #endregion
    #region header-outgoing-in-mutator
    public class SampleOutgoingMutator : IMutateOutgoingTransportMessages
    {
        public void MutateOutgoing(object[] messages, TransportMessage transportMessage)
        {
            Dictionary<string, string> headers = transportMessage.Headers;
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
            IDictionary<string, string> headers = Bus.CurrentMessageContext.Headers;
            string nsbVersion = headers[Headers.NServiceBusVersion];
            string customHeader = headers["MyCustomHeader"];
        }
    }
    #endregion
    #region header-outgoing-in-handler
    public class SampleWriteHandler : IHandleMessages<MyMessage>
    {
        public IBus Bus { get; set; }

        public void Handle(MyMessage message)
        {
            IDictionary<string, string> headers = Bus.OutgoingHeaders;
            headers["MyCustomHeader"] = "My custom value";
        }
    }
    #endregion
    internal class MyMessage
    {
    }

}
