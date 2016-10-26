namespace Core6.Handlers.DataAccess
{
    using System;

    public interface IMyOrmSession :
        IDisposable
    {
        Order Get(object orderId);
        void Commit();
    }
}