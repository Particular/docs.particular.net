using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NServiceBus.Logging;

class MySession :
    IMySession,
    IDisposable
{
    ILog log = LogManager.GetLogger<MySession>();
    List<object> entities = new List<object>();

    public void Dispose()
    {
    }

    public Task Store<T>(T entity)
    {
        entities.Add(entity);
        return Task.CompletedTask;
    }

    public Task Commit()
    {
        var entitiesStored = string.Join(", ", entities);
        log.Info($"Entities {entitiesStored} stored in DB by session {GetHashCode()}");
        return Task.CompletedTask;
    }

    public Task Rollback()
    {
        return Task.CompletedTask;
    }

    public override string ToString() => $"Session instance: {GetHashCode()}";

}