using NServiceBus;

namespace Messages
{
    public class TrackZipCode : ICommand
    {
        public string ZipCode { get; set; }
    }
}