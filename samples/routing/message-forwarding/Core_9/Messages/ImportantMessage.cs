using NServiceBus;

namespace Messages
{
    public class ImportantMessage :
        ICommand
    {
        public string Text { get; set; }
    }
}