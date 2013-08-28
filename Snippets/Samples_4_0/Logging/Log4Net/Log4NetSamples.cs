using System.Linq;
using log4net.Appender;
using log4net.Config;
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
    <log4net debug=""true"">
    <appender name=""MemoryAppender"" type=""log4net.Appender.MemoryAppender""/>
	    <root>
		    <level value=""DEBUG"" />
		    <appender-ref ref=""MemoryAppender"" />
	    </root>
    </log4net>
</configuration>";
            XmlConfigurator.Configure(appConfig.ToStream());

            SetLoggingLibrary.Log4Net();
            Configure.With()
                     .DefaultBuilder();

            var loggingEvents = Log4NetHelper.GetMessagesFromMemoryAppender()
                .ToList();
            Assert.IsNotEmpty(loggingEvents);
        }

        [Test]
        public void UsingCodeWithImplied()
        {
            var memoryAppender = new MemoryAppender();
            BasicConfigurator.Configure(memoryAppender);

            //This will log to all appenders currently configured in Log4net
            SetLoggingLibrary.Log4Net();
            Configure.With()
                     .DefaultBuilder();

            var loggingEvents = Log4NetHelper.GetMessagesFromMemoryAppender()
                .ToList();
            Assert.IsNotEmpty(loggingEvents);
        }

        [Test]
        public void UsingCodeWithSpecific()
        {
            var memoryAppender = new MemoryAppender();

            // This will configure log4net to log to the specified appender. 
            // Will also call BasicConfigurator.Configure(memoryAppender); internally
            Configure.With()
                     .Log4Net(memoryAppender)
                     .DefaultBuilder();

            var loggingEvents = Log4NetHelper.GetMessagesFromMemoryAppender()
                .ToList();
            Assert.IsNotEmpty(loggingEvents);
        }

    }

}
