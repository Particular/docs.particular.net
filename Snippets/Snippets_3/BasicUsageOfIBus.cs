namespace Snippets3
{
    using NServiceBus;
    using NServiceBus.Unicast.Config;

    public class BasicUsageOfIBus
    {
        void Send()
        {
            #region BasicSend
            ConfigUnicastBus configUnicastBus = Configure.With().UnicastBus();
            IBus bus = configUnicastBus.CreateBus().Start();

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