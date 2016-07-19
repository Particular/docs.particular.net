namespace Core5
{
    using NServiceBus;

    class BasicUsageOfIBus
    {
        void Send(BusConfiguration busConfiguration)
        {
            #region BasicSend
            var bus = Bus.Create(busConfiguration).Start();

            var myMessage = new MyMessage();
            bus.Send(myMessage);
            #endregion
        }

        #region SendFromHandler

        public class MyMessageHandler :
            IHandleMessages<MyMessage>
        {
            IBus bus;

            public MyMessageHandler(IBus bus)
            {
                this.bus = bus;
            }

            public void Handle(MyMessage message)
            {
                var otherMessage = new OtherMessage();
                bus.Send(otherMessage);
            }
        }
        #endregion

        void SendInterface(IBus bus)
        {
            #region BasicSendInterface
            bus.Send<IMyMessage>(m => m.MyProperty = "Hello world");
            #endregion
        }

        void SetDestination(IBus bus)
        {
            #region BasicSendSetDestination
            bus.Send(Address.Parse("MyDestination"), new MyMessage());
            #endregion
        }

        void ThisEndpoint(IBus bus)
        {
            #region BasicSendToAnyInstance
            var myMessage = new MyMessage();
            bus.SendLocal(myMessage);
            #endregion
        }

        void SendReplyToThisInstance(IBus bus)
        {
            #region BasicSendReplyToThisInstance
            var myMessage = new MyMessage();
            bus.SetMessageHeader(myMessage, NServiceBus.Headers.ReplyToAddress, "MyInstanceAddress");
            bus.Send(myMessage);
            #endregion
        }

        void SendReplyToAnyInstance(IBus bus)
        {
            #region BasicSendReplyToAnyInstance
            var myMessage = new MyMessage();
            bus.SetMessageHeader(myMessage, NServiceBus.Headers.ReplyToAddress, "MyEndpointAddress");
            bus.Send(myMessage);
            #endregion
        }

        void SendReplyTo(IBus bus)
        {
            #region BasicSendReplyToDestination
            var myMessage = new MyMessage();
            bus.SetMessageHeader(myMessage, NServiceBus.Headers.ReplyToAddress, "MyReplyDestination");
            bus.Send(myMessage);
            #endregion
        }

        void ReplySendReplyToThisInstance(IBus bus)
        {
            #region BasicReplyReplyToThisInstance
            var myMessage = new MyMessage();
            bus.SetMessageHeader(myMessage, NServiceBus.Headers.ReplyToAddress, "MyInstanceAddress");
            bus.Reply(myMessage);
            #endregion
        }

        void ReplySendReplyToAnyInstance(IBus bus)
        {
            #region BasicReplyReplyToAnyInstance
            var myMessage = new MyMessage();
            bus.SetMessageHeader(myMessage, NServiceBus.Headers.ReplyToAddress, "MyEndpointAddress");
            bus.Reply(myMessage);
            #endregion
        }

        void ReplySendReplyTo(IBus bus)
        {
            #region BasicReplyReplyToDestination
            var myMessage = new MyMessage();
            bus.SetMessageHeader(myMessage, NServiceBus.Headers.ReplyToAddress, "MyReplyDestination");
            bus.Reply(myMessage);
            #endregion
        }

        public class MyMessage
        {
        }

        public class OtherMessage
        {
        }

        interface IMyMessage
        {
            string MyProperty { get; set; }
        }
    }
}