namespace Snippets4
{
    using NServiceBus;

    public class BasicUsageOfIBus
    {
        void Send()
        {
            #region BasicSend
            IBus bus = CreateAndStartTheBus();

            bus.Send(new MyMessage());
            #endregion
        }

        #region SendFromHandler

        public class MyMessageHandler : IHandleMessages<MyMessage>
        {
            public IBus Bus { get; set; }

            public void Handle(MyMessage message)
            {
                Bus.Send(new OtherMessage());
            }
        }
        #endregion

        IBus CreateAndStartTheBus()
        {
            throw new System.NotImplementedException();
        }

        void SendInterface()
        {
            IBus bus = null;

            #region BasicSendInterface
            bus.Send<IMyMessage>(m => m.MyProperty = "Hello world");
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