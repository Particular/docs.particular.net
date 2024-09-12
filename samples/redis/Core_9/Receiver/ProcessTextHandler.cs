using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Shared.Messages;
using StackExchange.Redis;

class ProcessTextHandler(IDatabase db, ILogger<ProcessTextHandler> log) : IHandleMessages<ProcessText>
{
    public async Task Handle(ProcessText message, IMessageHandlerContext context)
    {
        #region get-string
        string text = await db.StringGetAsync(message.RedisKey);

        var mostCommonLetter = text.GroupBy(c => c).OrderByDescending(c => c.Count()).Select(c => c.Key).First();

        log.LogInformation("Most common letter in sample: {mostCommonLetter}", mostCommonLetter);
        #endregion

        #region expire-key
        await db.KeyExpireAsync(message.RedisKey, TimeSpan.FromMinutes(30));
        #endregion
    }
}
