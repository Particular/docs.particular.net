using System.Threading.Tasks;
using NServiceBus;

#region SendMessageJob
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
#endregion