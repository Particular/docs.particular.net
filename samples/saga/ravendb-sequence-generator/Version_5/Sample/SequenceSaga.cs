using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Saga;
using System;
using System.Threading;

#region TheSagaRavenDB

public class SequenceSaga : Saga<SequenceSagaData>,
	IAmStartedByMessages<IssueNext>
{
	static ILog logger = LogManager.GetLogger( typeof( SequenceSaga ) );

	protected override void ConfigureHowToFindSaga( SagaPropertyMapper<SequenceSagaData> mapper )
	{
		mapper.ConfigureMapping<IssueNext>( m => m.SequenceId ).ToSaga( s => s.SequenceId );
	}

	public void Handle( IssueNext message )
	{
		Data.SequenceId = message.SequenceId;
		Data.Latest = Data.Latest + 1;

		this.Bus.Reply( new NextNumber()
		{
			Value = Data.Latest,
			SequenceId = Data.SequenceId,
			Thread = Thread.CurrentThread.ManagedThreadId
		} );

		logger.InfoFormat( "SequenceSaga generated a new number '{0}' for sequence '{1}'.", Data.Latest, message.SequenceId );
	}
}

#endregion
