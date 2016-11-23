using System.Threading.Tasks;

class MySessionProvider
{
    public Task<MySession> Open(string tennant)
    {
        return Task.FromResult(new MySession(tennant));
    }
}