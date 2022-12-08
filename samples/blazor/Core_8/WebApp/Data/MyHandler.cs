using NServiceBus;
using System.Threading.Tasks;

namespace WebApp.Data
{
    public class MyHandler :
        IHandleMessages<MyMessage>
    {
        MyService myService;

        public MyHandler(MyService service)
        {
            myService = service;
        }
        public Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            myService.WriteHello();
            return Task.CompletedTask;
        }
    }
}
