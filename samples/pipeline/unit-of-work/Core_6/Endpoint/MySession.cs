using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NServiceBus.Logging;

class MySession :
    IMySession,
    IDisposable
{
    string tennant;
    ILog log = LogManager.GetLogger<MySession>();
    List<object> entities = new List<object>();

    public MySession(string tennant)
    {
        this.tennant = tennant;
    }

    public void Dispose()
    {
    }

    public Task Store<T>(T entity)
    {
        entities.Add(entity);
        return Task.FromResult(0);
    }

    public Task Commit()
    {
        var entitiesStored = string.Join(",", entities);
        log.Info($"{entitiesStored} stored in tennant database: {tennant}DB by session {GetHashCode()}");
        return Task.FromResult(0);
    }

    public Task Rollback()
    {
        return Task.FromResult(0);
    }

    public override string ToString() => $"Session for {tennant}(instance: {GetHashCode()})";

}