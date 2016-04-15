namespace Snippets5.DataBus.Custom
{
    using System;
    using System.IO;
    using NServiceBus.DataBus;

    #region CustomDataBus

    class CustomDataBus : IDataBus
    {
        public Stream Get(string key)
        {
            return File.OpenRead("blob.dat");
        }

        public string Put(Stream stream, TimeSpan timeToBeReceived)
        {
            using (FileStream destination = File.OpenWrite("blob.dat"))
            {
                stream.CopyTo(destination);
            }
            return "the-key-of-the-stored-file-such-as-the-full-path";
        }

        public void Start()
        {
        }
    }

    #endregion

}
