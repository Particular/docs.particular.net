using System.Threading.Tasks;

class MySessionProvider
{
    public Task<MySession> Open(string tenant)
    {
        return Task.FromResult(new MySession(tenant));
    }
}