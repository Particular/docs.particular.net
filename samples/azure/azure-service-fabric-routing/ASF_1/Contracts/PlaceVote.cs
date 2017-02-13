namespace Contracts
{
    using NServiceBus;

    public class PlaceVote : ICommand
    {
        public string ZipCode { get; set; }

        public string Candidate { get; set; }
    }
}
