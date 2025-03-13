using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

class MySession(ILogger<MySession> logger) :
    IMySession,
    IDisposable
{
    string tenant;
 
    readonly List<object> entities = [];

    public void Initialize(string tenantName)
    {
        tenant = tenantName;
        logger.LogInformation($"Initializing session for tenant {tenant}.");
    }

    public void Dispose()
    {
        logger.LogInformation($"Disposing session for tenant {tenant}.");
    }

    public Task Store<T>(T entity)
    {
        entities.Add(entity);
        return Task.CompletedTask;
    }

    public Task Commit()
    {
        var entitiesStored = string.Join(",", entities);
        logger.LogInformation($"{entitiesStored} stored in tenant database: {tenant}DB by session {GetHashCode()}");
        return Task.CompletedTask;
    }

    public Task Rollback()
    {
        return Task.CompletedTask;
    }

    public override string ToString() => $"Session for {tenant}(instance: {GetHashCode()})";

}