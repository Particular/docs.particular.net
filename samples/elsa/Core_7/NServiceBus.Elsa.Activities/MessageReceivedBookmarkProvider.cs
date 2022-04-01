using Elsa.Services;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NServiceBus.Activities
{
  internal class MessageReceivedBookmarkProvider : BookmarkProvider<MessageReceivedBookmark, NServiceBusMessageReceived>
  {
    public override async ValueTask<IEnumerable<BookmarkResult>> GetBookmarksAsync(BookmarkProviderContext<NServiceBusMessageReceived> context, CancellationToken cancellationToken)
    {
      string messageType = await context.ReadActivityPropertyAsync(x => x.MessageTypeQualifiedName, cancellationToken);

      return new[] { Result(new MessageReceivedBookmark(messageType)) };
    }
  }
}
