using NServiceBus;
using System;
using System.Threading.Tasks;

namespace Core7.Sagas.Timeouts
{
    public abstract class MultipleTimeoutsMySaga :
        Saga<MySagaData>,
        IAmStartedByMessages<Message1>
    {
        #region saga-multiple-timeouts
        public async Task Handle(Message1 message, IMessageHandlerContext context)
        {
            await RequestTimeout<MyCustomTimeout>(context, TimeSpan.FromHours(1));
            await RequestTimeout<MyCustomTimeout>(context, TimeSpan.FromDays(1));
            await RequestTimeout<MyOtherCustomTimeout>(context, TimeSpan.FromSeconds(10));
            await RequestTimeout<MyOtherCustomTimeout>(context, TimeSpan.FromMinutes(30));
        }
        #endregion
    }

    class MyOtherCustomTimeout { }
}