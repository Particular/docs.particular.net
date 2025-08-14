namespace Core.Sagas.FindSagas;

using System;
using System.Threading.Tasks;
using NServiceBus.Persistence;

public static class SynchronizedStorageSessionEx
{
    public static DbSession GetDbSession(this ISynchronizedStorageSession session)
    {
        throw new NotImplementedException();
    }
}

public class DbSession
{
    public Task<MySagaData> GetSagaFromDB(string messageSomeId, string messageSomeData)
    {
        throw new NotImplementedException();
    }
}