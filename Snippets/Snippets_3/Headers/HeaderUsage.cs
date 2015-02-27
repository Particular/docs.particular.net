using System.Collections.Generic;
using NServiceBus;
using NServiceBus.MessageMutator;
using NServiceBus.Unicast.Transport;

class HeaderUsage
{
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
        public void MutateOutgoing(object[] messages, TransportMessage transportMessage)
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
