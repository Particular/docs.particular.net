using NServiceBus;
using System;

public class NextNumber : IMessage
{
	public int Value { get; set; }

	public string SequenceId { get; set; }

	public int Thread { get; set; }
}