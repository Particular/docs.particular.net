namespace Snippets6
{
    using System.Threading.Tasks;
    using NServiceBus;

    public class BasicUsageOfIBus
    {
        async Task Send()
        {
            IBusContext busContext = null;

            #region BasicSend
            await busContext.Send(new MyMessage());
            #endregion
        }

        async Task SendInterface()
        {
            IBusContext busContext = null;

            #region BasicSendInterface
            await busContext.Send<IMyMessage>(m => m.MyProperty = "Hello world");
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