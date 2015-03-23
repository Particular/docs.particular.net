using System;
using NServiceBus.Saga;

#region OrderSagaDataRavenDB

public class SequenceSagaData : IContainSagaData
{
    public Guid Id { get; set; }
    public string Originator { get; set; }
    public string OriginalMessageId { get; set; }

	[Unique]
    public string SequenceId { get; set; }

	public int Latest { get; set; }
}

#endregion