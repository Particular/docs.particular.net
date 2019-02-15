using System;
using Raven.Client.Listeners;
using Raven.Json.Linq;

#region DocumentStoreListener
class MarkForExpiryListener : IDocumentStoreListener
{
    public bool BeforeStore(string key, object entityInstance, RavenJObject metadata, RavenJObject original)
    {
        if (entityInstance.GetType().Name != "OutboxRecord")
        {
            return false;
        }

        var dispatched = entityInstance.GetPropertyValue<bool>("Dispatched");
        if (dispatched)
        {
            var dispatchedAt = entityInstance.GetPropertyValue<DateTime>("DispatchedAt");
            var expiry = dispatchedAt.AddDays(10);
            metadata["Raven-Expiration-Date"] = new RavenJValue(expiry);
        }
        return false;
    }

    public void AfterStore(string key, object entityInstance, RavenJObject metadata)
    {
    }
}
#endregion