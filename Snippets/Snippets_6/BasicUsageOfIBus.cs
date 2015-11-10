namespace Snippets6
{
    using System.Threading.Tasks;
    using NServiceBus;

    public class BasicUsageOfIBus
    {
        async Task Send()
        {
            IBus bus = null;

            #region BasicSend
            await bus.SendAsync(new MyMessage());
            #endregion
        }

        async Task SendInterface()
        {
            IBus bus = null;

            #region BasicSendInterface
            await bus.SendAsync<IMyMessage>(m => m.MyProperty = "Hello world");
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