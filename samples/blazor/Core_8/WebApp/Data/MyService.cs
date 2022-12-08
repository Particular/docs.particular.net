using NServiceBus.Logging;

namespace WebApp.Data
{
    public class MyService
    {
        static ILog log = LogManager.GetLogger<MyService>();

        public void WriteHello()
        {
            log.Info("Hello from MyService.");
        }
    }
}
