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

    void ExecuteAtStartup(EndpointConfiguration endpointConfiguration)
    {
        #region ExecuteAtStartup

        endpointConfiguration.EnableInstallers();
        var attachments = endpointConfiguration.EnableAttachments(
            fileShare: "networkSharePath",
            timeToKeep: TimeToKeep.Default);

        #endregion
    }

    void DisableInstaller(EndpointConfiguration endpointConfiguration)
    {
        #region DisableInstaller

        endpointConfiguration.EnableInstallers();
        var attachments = endpointConfiguration.EnableAttachments(
            fileShare: "networkSharePath",
            timeToKeep: TimeToKeep.Default);
        attachments.DisableInstaller();

        #endregion
    }
}