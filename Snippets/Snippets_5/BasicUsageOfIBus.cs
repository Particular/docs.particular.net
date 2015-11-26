namespace Snippets5
{
    using NServiceBus;

    public class BasicUsageOfIBus
    {
        void Send()
        {
            var busConfig = new BusConfiguration();

            #region BasicSend
            IBus bus = Bus.Create(busConfig).Start();

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