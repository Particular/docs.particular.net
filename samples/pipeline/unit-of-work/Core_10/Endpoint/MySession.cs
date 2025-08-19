using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

class MySession(ILogger<MySession> logger) :
    IMySession,
    IDisposable
{

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
        logger.LogInformation("Entities {EntitiesStored} stored in DB by session {SessionHash}", entitiesStored, GetHashCode());
        return Task.CompletedTask;
    }

    public Task Rollback()
    {
        return Task.CompletedTask;
    }

    public override string ToString() => $"Session instance: {GetHashCode()}";

}