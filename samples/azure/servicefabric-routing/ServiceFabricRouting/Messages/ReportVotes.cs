using NServiceBus;

namespace Messages
{
    public class ReportVotes : ICommand
    {
        public string Candidate { get; set; }

        public int NumberOfVotes { get; set; }
    }
}