using System;
using NServiceBus.Saga;

#region OrderSagaDataRavenDB

public class SequenceSagaData : ContainSagaData
{
	[Unique]
	public string SequenceId { get; set; }

	public int Latest { get; set; }
}

#endregion