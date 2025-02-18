namespace Core7.DataBus.Custom
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using NServiceBus.DataBus;

    #region CustomDataBus

    class CustomDataBus :
        IDataBus
    {
        public Task<Stream> Get(string key)
        {
            Stream stream = File.OpenRead("blob.dat");
            return Task.FromResult(stream);
        }

        public async Task<string> Put(Stream stream, TimeSpan timeToBeReceived)
        {
            using (var destination = File.OpenWrite("blob.dat"))
            {
                await stream.CopyToAsync(destination);
            }
            return "the-key-of-the-stored-file-such-as-the-full-path";
        }

        public Task Start()
        {
            return Task.CompletedTask;
        }
    }

    #endregion

}
