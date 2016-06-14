using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

class NoOpSubscribeTerminator : PipelineTerminator<ISubscribeContext>
{
    protected override Task Terminate(ISubscribeContext context)
    {
        return Task.FromResult(0); 
    }
}