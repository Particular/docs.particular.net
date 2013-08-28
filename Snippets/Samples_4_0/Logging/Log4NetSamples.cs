using System.IO;
using System.Threading;
using log4net.Config;
using NServiceBus;
using NServiceBus.Logging;
using NUnit.Framework;

namespace Snippets_4_0.Logging
{
    class Log4NestSamples
    {
        [Test]
        public void UsingConfig()
        {
            var appConfigPath = Path.Combine(AssemblyLocation.CurrentDirectory(), @"Logging\Log4Net\app.config");
            XmlConfigurator.Configure(new FileInfo(appConfigPath));
            //var traceListener = new ActionTraceListener();
            //Trace.Listeners.Add(traceListener);

            using (var bus = Configure
                .With()
                .DefaultBuilder()
                .UnicastBus()
                .CreateBus())
            {
                bus.Start();
                bus.SendLocal(new MyMessage());
                Thread.Sleep(1000);
                //Debug.WriteLine(traceListener.messages);
            }
        }
    }

    public class MyHandler : IHandleMessages<MyMessage>
    {
        static readonly ILog Logger = LogManager.GetLogger(typeof(MyHandler));
        public void Handle(MyMessage message)
        {
            Logger.Info("Hello");
        }
    }

    public class MyMessage:IMessage
    {
    }
}
