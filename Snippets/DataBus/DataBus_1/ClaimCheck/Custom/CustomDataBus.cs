namespace ClaimCheck_1.ClaimCheck.Custom;

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus.ClaimCheck;

#region CustomDataBus

class CustomClaimCheck :
    IClaimCheck
{
    public Task<Stream> Get(string key, CancellationToken cancellationToken)
    {
        Stream stream = File.OpenRead("blob.dat");
        return Task.FromResult(stream);
    }

    public async Task<string> Put(Stream stream, TimeSpan timeToBeReceived, CancellationToken cancellationToken)
    {
        await using var destination = File.OpenWrite("blob.dat");
        await stream.CopyToAsync(destination, 81920, cancellationToken);
        return "the-key-of-the-stored-file-such-as-the-full-path";
    }

    public Task Start(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

#endregion