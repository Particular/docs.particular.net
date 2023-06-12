namespace Core8.DataBus.Custom
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using NServiceBus.DataBus;

    #region CustomDataBus

    class CustomDataBus :
        IDataBus
    {
        public Task<Stream> Get(string key, CancellationToken cancellationToken)
        {
            Stream stream = File.OpenRead("blob.dat");
            return Task.FromResult(stream);
        }

        public async Task<string> Put(Stream stream, TimeSpan timeToBeReceived, CancellationToken cancellationToken)
        {
            using (var destination = File.OpenWrite("blob.dat"))
            {
                await stream.CopyToAsync(destination, 81920, cancellationToken)
                    .ConfigureAwait(false);
            }
            return "the-key-of-the-stored-file-such-as-the-full-path";
        }

        public Task Start(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

    #endregion

}
