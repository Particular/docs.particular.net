using System;
using NServiceBus;
using NServiceBus.Attachments.FileShare;

public class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region EnableAttachments

        endpointConfiguration.EnableAttachments(
            fileShare: "networkSharePath",
            timeToKeep: messageTimeToBeReceived => TimeSpan.FromDays(7));

        #endregion

        #region EnableAttachmentsRecommended

        endpointConfiguration.EnableAttachments(
            fileShare: "networkSharePath",
            timeToKeep: TimeToKeep.Default);

        #endregion
    }

    void DisableCleanupTask(EndpointConfiguration endpointConfiguration)
    {
        #region DisableCleanupTask

        var attachments = endpointConfiguration.EnableAttachments(
            fileShare: "networkSharePath",
            timeToKeep: TimeToKeep.Default);
        attachments.DisableCleanupTask();

        #endregion
    }
}