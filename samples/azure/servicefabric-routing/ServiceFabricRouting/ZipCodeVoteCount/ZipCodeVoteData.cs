using NServiceBus;

namespace ZipCodeVoteCount
{
    public class ZipCodeVoteData : ContainSagaData
    {
        public string ZipCode { get; set; }
        public int Count { get; set; }
        public bool Started { get; set; }
    }
}