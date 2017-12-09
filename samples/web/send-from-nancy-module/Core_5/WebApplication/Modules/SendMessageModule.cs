using Nancy;
using NServiceBus;

namespace WebApplication.Modules
{
    public class SendMessageModule : NancyModule
    {
        private readonly ISendOnlyBus bus;

        public SendMessageModule(ISendOnlyBus bus) : base("/sendmessage")
        {
            this.bus = bus;

            this.Get["/"] = r =>
            {
                var message = new MyMessage();
                bus.Send("Samples.Nancy.Endpoint", message);
                return "Message sent to endpoint";
            };
        }
    }
}