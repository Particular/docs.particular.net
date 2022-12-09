using NServiceBus;
using System.Threading.Tasks;

namespace WebApp.Data
{
    //#region InjectingMessageSession
    public class MessageSenderService
    {
        private readonly IMessageSession messageSession;

        public MessageSenderService(IMessageSession messageSession)
        {
            this.messageSession = messageSession;
        }

        public Task SendMessage()
        {
            var myMessage = new MyMessage();
            return messageSession.SendLocal(myMessage);
        }
    }
    //#endregion
}
