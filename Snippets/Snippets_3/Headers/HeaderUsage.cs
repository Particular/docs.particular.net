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
            SomeOtherMessage someOtherMessage = new SomeOtherMessage();
            someOtherMessage.SetHeader("MyCustomHeader", "My custom value");
            bus.Send(someOtherMessage);
        }
    }
    #endregion
    class SomeOtherMessage
    {
    }
    internal class MyMessage
    {
    }

}
