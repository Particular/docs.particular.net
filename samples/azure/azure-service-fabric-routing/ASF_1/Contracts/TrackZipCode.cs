namespace Contracts
{
    using NServiceBus;

    public class TrackZipCode : ICommand, IHaveZipCode
    {
        public string ZipCode { get; set; }
    }
}