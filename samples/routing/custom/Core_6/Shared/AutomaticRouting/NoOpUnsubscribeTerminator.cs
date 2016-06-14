using System.Threading.Tasks;
using NServiceBus.Pipeline;

class NoOpUnsubscribeTerminator : PipelineTerminator<IUnsubscribeContext>
{
    protected override Task Terminate(IUnsubscribeContext context)
    {
        return Task.FromResult(0);
    }
}