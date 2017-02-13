using NServiceBus;

namespace Messages
{
    public class TrackZipCode : ICommand, IHaveZipCode
    {
        public string ZipCode { get; set; }
    }
}