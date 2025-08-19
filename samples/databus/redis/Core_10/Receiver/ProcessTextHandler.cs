using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Shared.Messages;

class ProcessTextHandler(ILogger<ProcessTextHandler> log) : IHandleMessages<ProcessText>
{
    #region process-message
    public Task Handle(ProcessText message, IMessageHandlerContext context)
    {
        log.LogInformation(
            "Most common letter in sample: {mostCommonLetter}",
            MostCommonLetter(message.LargeText)
        );

        return Task.CompletedTask;
    }
    #endregion

    private static char MostCommonLetter(string text) => text.GroupBy(c => c).OrderByDescending(c => c.Count()).Select(c => c.Key).First();
}
