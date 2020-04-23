using System;
using NServiceBus.Saga;

namespace Core5.Sagas.Timeouts
{
    public abstract class MultipleTimeoutsMySaga :
        Saga<MySagaData>,
        IAmStartedByMessages<Message1>
    {
        #region saga-multiple-timeouts
        public void Handle(Message1 message)
        {
            RequestTimeout<MyCustomTimeout>(TimeSpan.FromHours(1));
            RequestTimeout<MyCustomTimeout>(TimeSpan.FromDays(1));
            RequestTimeout<MyOtherCustomTimeout>(TimeSpan.FromSeconds(10));
            RequestTimeout<MyOtherCustomTimeout>(TimeSpan.FromMinutes(30));
        }
        #endregion
    }

    class MyOtherCustomTimeout { }
}