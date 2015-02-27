using System;
using System.Collections.Generic;
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
            Dictionary<string, string> headers = context.PhysicalMessage.Headers;
            string nsbVersion = headers[Headers.NServiceBusVersion];
            string customHeader = headers["MyCustomHeader"];
            next();
        }
    }
    #endregion
    #region header-outgoing-behavior
    public class SampleOutgoingBehavior : IBehavior<OutgoingContext>
    {
        public void Invoke(OutgoingContext context, Action next)
        {
            Dictionary<string, string> headers = context.OutgoingMessage.Headers;
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
            Dictionary<string, string> headers = transportMessage.Headers;
            string nsbVersion = headers[Headers.NServiceBusVersion];
            string customHeader = headers["MyCustomHeader"];
        }
    }
    #endregion
    #region header-outgoing-mutator
    public class SampleOutgoingMutator : IMutateOutgoingTransportMessages
    {
        public void MutateOutgoing(LogicalMessage logicalMessage, TransportMessage transportMessage)
        {
            Dictionary<string, string> headers = transportMessage.Headers;
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
            IDictionary<string, string> headers = Bus.CurrentMessageContext.Headers;
            string nsbVersion = headers[Headers.NServiceBusVersion];
            string customHeader = headers["MyCustomHeader"];
        }
    }
    #endregion
    #region header-outgoing-handler
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
