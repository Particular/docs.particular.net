using Elsa.Services;

namespace NServiceBus.Activities
{
    internal class MessageReceivedBookmarkProvider : BookmarkProvider<MessageReceivedBookmark, NServiceBusMessageReceived>
    {
        /// <summary>
        /// Elsa requires a bookmark provider/factory for custom bookmarks.
        /// </summary>        
        public override async ValueTask<IEnumerable<BookmarkResult>> GetBookmarksAsync(BookmarkProviderContext<NServiceBusMessageReceived> context, CancellationToken cancellationToken)
        {
            string? messageType = await context.ReadActivityPropertyAsync(x => x.MessageTypeQualifiedName, cancellationToken);

            return new[] { Result(new MessageReceivedBookmark(messageType)) };
        }
    }
}
