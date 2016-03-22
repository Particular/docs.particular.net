namespace Snippets4.DataBus.CleanupStrategy
{
    using System;
    using System.IO;
    using Snippets4.DataBus.DataBusProperty;

    public class Usage
    {
        #region FileLocationForDatabusFiles
        public void Handle(MessageWithLargePayload message)
        {
            string filename = Path.Combine(@"c:\database_files\", message.LargeBlob.Key);
            Console.WriteLine(filename);
        }
        #endregion
    }
}
