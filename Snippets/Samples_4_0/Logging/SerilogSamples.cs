using System.Collections.Generic;
using NServiceBus;
using NServiceBus.Serilog;
using NUnit.Framework;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Snippets_4_0.Logging
{
    class SerilogSamples
    {

        [Test]
        public void UsingCode()
        {
            var sink = new MySink();
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Sink(sink)
                .CreateLogger();
            SerilogConfigurator.Configure();

            Configure.With()
                     .DefaultBuilder();

            Assert.IsNotEmpty(sink.LogEvents);
        }

    }

    class MySink : ILogEventSink
    {
        public List<LogEvent> LogEvents = new List<LogEvent>();

        public void Emit(LogEvent logEvent)
        {
            LogEvents.Add(logEvent);
        }
    }
}
