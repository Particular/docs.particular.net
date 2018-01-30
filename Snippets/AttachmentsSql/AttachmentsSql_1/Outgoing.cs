using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using NServiceBus;

public class Outgoing
{
    #region OutgoingFactory

    class HandlerFactory :
        IHandleMessages<MyMessage>
    {
        public Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            var sendOptions = new SendOptions();
            var outgoingAttachments = sendOptions.Attachments();
            outgoingAttachments.Add(
                name: "attachment1",
                stream: () => File.OpenRead("FilePath.txt"));
            return context.Send(new OtherMessage(), sendOptions);
        }
    }

    #endregion

    #region OutgoingFactoryAsync

    class HandlerFactoryAsync :
        IHandleMessages<MyMessage>
    {
        static HttpClient httpClient = new HttpClient();

        public Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            var sendOptions = new SendOptions();
            var outgoingAttachments = sendOptions.Attachments();
            outgoingAttachments.Add(
                name: "attachment1",
                stream: () => httpClient.GetStreamAsync("theUrl"));
            return context.Send(new OtherMessage(), sendOptions);
        }
    }

    #endregion

    #region OutgoingInstance

    class HandlerInstance :
        IHandleMessages<MyMessage>
    {
        public Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            var sendOptions = new SendOptions();
            var outgoingAttachments = sendOptions.Attachments();
            var stream = File.OpenRead("FilePath.txt");
            outgoingAttachments.Add(
                name: "attachment1",
                stream: stream,
                cleanup: () => File.Delete("FilePath.txt"));
            return context.Send(new OtherMessage(), sendOptions);
        }
    }

    #endregion
}