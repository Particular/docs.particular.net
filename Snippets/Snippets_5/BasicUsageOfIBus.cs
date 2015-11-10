namespace Snippets5
{
    using NServiceBus;

    public class BasicUsageOfIBus
    {
        void Send()
        {
            IBus bus = null;

            #region BasicSend
            bus.Send(new MyMessage());
            #endregion
        }

        void SendInterface()
        {
            IBus bus = null;

            #region BasicSendInterface
            bus.Send<IMyMessage>(m => m.MyProperty = "Hello world");
            #endregion
        }

        class MyMessage
        {
        }

        interface IMyMessage
        {
            string MyProperty { get; set; }
        }
    }
}