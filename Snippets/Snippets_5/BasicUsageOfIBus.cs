namespace Snippets5
{
    using NServiceBus;

    public class BasicUsageOfIBus
    {
        void Send()
        {
            var busConfiguration = new BusConfiguration();

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