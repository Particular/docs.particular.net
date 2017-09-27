using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NServiceBus.Logging;

class MySession :
    IMySession,
    IDisposable
{
    string tenant;
    ILog log = LogManager.GetLogger<MySession>();
    List<object> entities = new List<object>();

    public MySession(string tenant)
    {
        this.tenant = tenant;
    }

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
        var entitiesStored = string.Join(",", entities);
        log.Info($"{entitiesStored} stored in tenant database: {tenant}DB by session {GetHashCode()}");
        return Task.CompletedTask;
    }

    public Task Rollback()
    {
        return Task.CompletedTask;
    }

    public override string ToString() => $"Session for {tenant}(instance: {GetHashCode()})";

}