using NServiceBus;

public class IssueNext : IMessage
{
	public string SequenceId { get; set; }
}