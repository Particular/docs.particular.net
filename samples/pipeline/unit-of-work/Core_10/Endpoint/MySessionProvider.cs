using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

class MySessionProvider
{
    private readonly ILogger<MySession> logger;

    public MySessionProvider(ILogger<MySession> logger)
    {
        this.logger = logger;
    }
    public Task<MySession> Open()
    {
        return Task.FromResult(new MySession(logger));
    }
}