using System;
using System.IO;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region read

public class MyHandler :
    IHandleMessages<MyMessage>
{
    static ILog log = LogManager.GetLogger<MyHandler>();

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        log.Info("MyMessage received. Attachment will be streamed to the console.");
        var attachments = context.Attachments();
        return attachments.ProcessStream(
            name: "attachmentName",
            action: async stream =>
            {
                using (var reader = new StreamReader(stream))
                {
                    while (true)
                    {
                        var line = await reader.ReadLineAsync()
                            .ConfigureAwait(false);
                        if (line == null)
                        {
                            break;
                        }

                        Console.WriteLine(line);
                    }
                }
            });
    }
}

#endregion