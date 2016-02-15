namespace Snippets5
{
    using NServiceBus;

    public class BasicUsageOfIBus
    {
        void Send()
        {
            BusConfiguration busConfiguration = new BusConfiguration();

            #region BasicSend
            IBus bus = Bus.Create(busConfiguration).Start();

            bus.Send(new MyMessage());
            #endregion
        }

        #region SendFromHandler

        public class MyMessageHandler : IHandleMessages<MyMessage>
        {
            IBus bus;

            public MyMessageHandler(IBus bus)
            {
                this.bus = bus;
            }

            public void Handle(MyMessage message)
            {
                bus.Send(new OtherMessage());
            }
        }
        #endregion

        void SendInterface()
        {
            IBus bus = null;

            #region BasicSendInterface
            bus.Send<IMyMessage>(m => m.MyProperty = "Hello world");
            #endregion
        }

        void SetDestination()
        {
            IBus bus = null;

            #region BasicSendSetDestination
            bus.Send(Address.Parse("MyDestination"), new MyMessage());
            #endregion
        }

        void ThisEndpoint()
        {
            IBus bus = null;

            #region BasicSendToAnyInstance
            bus.SendLocal(new MyMessage());
            #endregion
        }

        void SendReplyToThisInstance()
        {
            IBus bus = null;

            #region BasicSendReplyToThisInstance
            MyMessage myMessage = new MyMessage();
            bus.SetMessageHeader(myMessage, NServiceBus.Headers.ReplyToAddress, "MyInstanceAddress");
            bus.Send(myMessage);
            #endregion
        }

        void SendReplyToAnyInstance()
        {
            IBus bus = null;

            #region BasicSendReplyToAnyInstance
            MyMessage myMessage = new MyMessage();
            bus.SetMessageHeader(myMessage, NServiceBus.Headers.ReplyToAddress, "MyEndpointAddress");
            bus.Send(myMessage);
            #endregion
        }

        void SendReplyTo()
        {
            IBus bus = null;

            #region BasicSendReplyToDestination
            MyMessage myMessage = new MyMessage();
            bus.SetMessageHeader(myMessage, NServiceBus.Headers.ReplyToAddress, "MyReplyDestination");
            bus.Send(myMessage);
            #endregion
        }

        void ReplySendReplyToThisInstance()
        {
            IBus bus = null;

            #region BasicReplyReplyToThisInstance
            MyMessage myMessage = new MyMessage();
            bus.SetMessageHeader(myMessage, NServiceBus.Headers.ReplyToAddress, "MyInstanceAddress");
            bus.Reply(myMessage);
            #endregion
        }

        void ReplySendReplyToAnyInstance()
        {
            IBus bus = null;

            #region BasicReplyReplyToAnyInstance
            MyMessage myMessage = new MyMessage();
            bus.SetMessageHeader(myMessage, NServiceBus.Headers.ReplyToAddress, "MyEndpointAddress");
            bus.Reply(myMessage);
            #endregion
        }

        void ReplySendReplyTo()
        {
            IBus bus = null;

            #region BasicReplyReplyToDestination
            MyMessage myMessage = new MyMessage();
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