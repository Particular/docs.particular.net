﻿using System.IO;
using System.Threading.Tasks;
using NServiceBus;

public class Incoming
{
    #region ProcessStream

    class HandlerProcessStream :
        IHandleMessages<MyMessage>
    {
        public async Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            var incomingAttachments = context.Attachments();
            await incomingAttachments.ProcessStream(
                name: "attachment1",
                action: async stream =>
                {
                    // Use the attachment stream. in this example copy to a file
                    using (var fileToCopyTo = File.Create("FilePath.txt"))
                    {
                        await stream.CopyToAsync(fileToCopyTo).ConfigureAwait(false);
                    }
                });
        }
    }

    #endregion

    #region ProcessStreams

    class HandlerProcessStreams :
        IHandleMessages<MyMessage>
    {
        public async Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            var incomingAttachments = context.Attachments();
            await incomingAttachments.ProcessStreams(
                    action: async (name, stream) =>
                    {
                        // Use the attachment stream. in this example copy to a file
                        using (var fileToCopyTo = File.Create($"{name}.txt"))
                        {
                            await stream.CopyToAsync(fileToCopyTo)
                                .ConfigureAwait(false);
                        }
                    })
                .ConfigureAwait(false);
        }
    }

    #endregion

    #region ProcessStreamsForMessage

    class HandlerProcessStreamsForMessage :
        IHandleMessages<MyMessage>
    {
        public async Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            var incomingAttachments = context.Attachments();
            await incomingAttachments.ProcessStreamsForMessage(
                    messageId: "theMessageId",
                    action: async (name, stream) =>
                    {
                        // Use the attachment stream. in this example copy to a file
                        using (var fileToCopyTo = File.Create($"{name}.txt"))
                        {
                            await stream.CopyToAsync(fileToCopyTo)
                                .ConfigureAwait(false);
                        }
                    })
                .ConfigureAwait(false);
        }
    }

    #endregion

    #region CopyTo

    class HandlerCopyTo :
        IHandleMessages<MyMessage>
    {
        public async Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            var incomingAttachments = context.Attachments();
            using (var fileToCopyTo = File.Create("FilePath.txt"))
            {
                await incomingAttachments.CopyTo("attachment1", fileToCopyTo)
                    .ConfigureAwait(false);
            }
        }
    }

    #endregion

    #region GetBytes

    class HandlerGetBytes :
        IHandleMessages<MyMessage>
    {
        public async Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            var incomingAttachments = context.Attachments();
            var bytes = await incomingAttachments.GetBytes("attachment1")
                .ConfigureAwait(false);
            // use the byte array
        }
    }

    #endregion

    #region GetStream

    class HandlerGetStream :
        IHandleMessages<MyMessage>
    {
        public async Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            var incomingAttachments = context.Attachments();
            using (var attachmentStream = await incomingAttachments.GetStream("attachment1").ConfigureAwait(false))
            {
                // Use the attachment stream. in this example copy to a file
                using (var fileToCopyTo = File.Create("FilePath.txt"))
                {
                    await attachmentStream.CopyToAsync(fileToCopyTo)
                        .ConfigureAwait(false);
                }
            }
        }
    }

    #endregion
}