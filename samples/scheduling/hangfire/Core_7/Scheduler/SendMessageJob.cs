using System.Threading.Tasks;
using NServiceBus;

namespace Scheduler
{
    public class SendMessageJob
    {
        public static Task Run()
        {
            return EndpointHelper.Instance.Send("Samples.HangfireScheduler.Receiver", new MyMessage());
        }
    }
}
