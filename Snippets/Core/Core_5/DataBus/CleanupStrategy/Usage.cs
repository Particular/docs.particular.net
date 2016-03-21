namespace Snippets5.DataBus.CleanupStrategy
{
    using System;
    using NServiceBus;
    using Core5.DataBus.DataBusProperty;

    public class Usage
    {
        private IBus Bus = null;

        #region FileLocationForDatabusFiles
        public void Handle(MessageWithLargePayload message)
        {
            string filename = message.LargeBlob.Key;
            Bus.Defer(TimeSpan.FromDays(30), new RemoveDatabusAttachment(filename));
        }

        public void Handle(RemoveDatabusAttachment message)
        {
            CleanUp(message.Filename);
        }

        private void CleanUp(string filename)
        {
        }

        #endregion
    }

    public class RemoveDatabusAttachment : ICommand
    {
        public RemoveDatabusAttachment(string filename)
        {
            Filename = filename;
        }

        public string Filename { get; set; }
    }
}
