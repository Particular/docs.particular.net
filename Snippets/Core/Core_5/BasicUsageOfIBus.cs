﻿namespace Core5
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

            bus.Send<IMyMessage>(
                messageConstructor: message =>
                {
                    message.MyProperty = "Hello world";
                });

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