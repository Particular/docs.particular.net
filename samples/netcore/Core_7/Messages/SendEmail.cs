using NServiceBus;

namespace Messages
{
    public class SendEmail : ICommand
    {
        public string CustomerName { get; set; }
    }
}
