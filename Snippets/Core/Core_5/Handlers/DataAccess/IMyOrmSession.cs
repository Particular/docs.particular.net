namespace Core5.Handlers
{
    using System;

    public interface IMyOrmSession : IDisposable
    {
        Order Get(object orderId);
        void Commit();
    }
}