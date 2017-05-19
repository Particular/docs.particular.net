using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Sagas;

class MyMessageHandler : IHandleMessages<MyMessage>
{
    MyFacade facade;

    public MyMessageHandler(MyFacade facade)
    {
        this.facade = facade;
    }

    public async Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        await Task.Yield(); //Ensure the current culture can be accessed.
        facade.Do(message.Value);
    }
}