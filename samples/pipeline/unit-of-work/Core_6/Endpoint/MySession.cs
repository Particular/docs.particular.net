using System;
using System.Threading.Tasks;

class MySession : IMySession,
    IDisposable
{
    readonly string tennant;

    public MySession(string tennant)
    {
        this.tennant = tennant;
    }

    public void Dispose()
    {
    }

    public Task Store<T>(T myEntity)
    {
        Console.Out.WriteLine($"{typeof(T)} stored in tennant database: {tennant}DB");
        return Task.FromResult(0);
    }

    public Task Commit()
    {
        return Task.FromResult(0);
    }

    public Task Rollback()
    {
        return Task.FromResult(0);
    }
}