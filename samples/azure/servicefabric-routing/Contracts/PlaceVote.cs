using NServiceBus;

namespace Messages
{
    public class PlaceVote : ICommand, IHaveCandidate
    {
        public string ZipCode { get; set; }

        public string Candidate { get; set; }
    }
}
