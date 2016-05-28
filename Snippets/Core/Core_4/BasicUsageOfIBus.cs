namespace Core4
{
    using NServiceBus;

    class BasicUsageOfIBus
    {
        void Send()
        {
            #region BasicSend
            var configUnicastBus = Configure.With().UnicastBus();
            var bus = configUnicastBus.CreateBus().Start();

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
            bus.SendLocal(new MyMessage());
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