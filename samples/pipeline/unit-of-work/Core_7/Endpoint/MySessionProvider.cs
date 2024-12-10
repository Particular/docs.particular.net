using System.Threading.Tasks;

class MySessionProvider
{
    public Task<MySession> Open()
    {
        return Task.FromResult(new MySession());
    }
}