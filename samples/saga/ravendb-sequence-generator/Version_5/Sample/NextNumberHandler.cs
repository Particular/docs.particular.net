using NServiceBus;
using NServiceBus.Logging;
using System;

class NextNumberHandler : IHandleMessages<NextNumber>
{
	static ILog logger = LogManager.GetLogger( typeof( NextNumberHandler ) );

	public void Handle( NextNumber message )
	{
		logger.InfoFormat( "NextNumber for sequence '{0}' is '{1}', generated on thread '{2}'", message.SequenceId, message.Value, message.Thread );
	}
}
