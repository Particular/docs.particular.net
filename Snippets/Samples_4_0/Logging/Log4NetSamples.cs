using System.Collections.Generic;
using System.Linq;
using log4net.Appender;
using log4net.Config;
using log4net.Repository.Hierarchy;
using NServiceBus;
using NUnit.Framework;

namespace Snippets_4_0.Logging
{
    class Log4NetSamples
    {
        [Test]
        public void UsingAppConfig()
        {
            //This would be the contents of your app.config file
            var appConfig =
@"<?xml version=""1.0"" encoding=""utf-8"" ?>
<configuration>
    <configSections>
	    <section name=""log4net"" type=""log4net.Config.Log4NetConfigurationSectionHandler, log4net"" />
    </configSections>
    <log4net>
        <appender name=""appender"" type=""log4net.Appender.MemoryAppender""/>
	    <root>
		    <level value=""DEBUG"" />
		    <appender-ref ref=""appender"" />
	    </root>
    </log4net>
</configuration>";
            //A helper method so we can load the above appconfig into Log4Net
            XmlConfigurator.Configure(appConfig.ToStream());

            SetLoggingLibrary.Log4Net();
            Configure.With()
                     .DefaultBuilder();

            var loggingEvents = GetMessagesFromMemoryAppender()
                .ToList();
            Assert.IsNotEmpty(loggingEvents);
        }

        [Test]
        public void UsingCodeWithImplied()
        {
            var appender = new MemoryAppender();
            BasicConfigurator.Configure(appender);

            //This will log to all appenders currently configured in Log4net
            SetLoggingLibrary.Log4Net();
            Configure.With()
                     .DefaultBuilder();

            var loggingEvents = appender.GetEvents()
                .ToList();
            Assert.IsNotEmpty(loggingEvents);
        }

        [Test]
        public void UsingCodeWithSpecific()
        {
            var appender = new MemoryAppender();

            // This will configure log4net to log to the specified appender. 
            // Will also call BasicConfigurator.Configure(memoryAppender); internally
            Configure.With()
                     .Log4Net(appender)
                     .DefaultBuilder();

            var loggingEvents = appender.GetEvents()
                .ToList();
            Assert.IsNotEmpty(loggingEvents);
        }

        static IEnumerable<string> GetMessagesFromMemoryAppender()
        {
            var repository = (Hierarchy)log4net.LogManager.GetRepository();
            var memoryAppender = (MemoryAppender)repository.Root.Appenders[0];
            return memoryAppender.GetEvents().Select(x => x.RenderedMessage);
        }
    }

}
