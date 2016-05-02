#pragma warning disable 414
using System.Collections.Generic;
using System.Linq;
using Raven.Abstractions.Data;
using Raven.Abstractions.Extensions;
using Raven.Client;
using Raven.Client.Connection;
using Raven.Client.Document;
using Raven.Json.Linq;

public static class UserCreation
{

    static void Usage()
    {
        #region raven-add-user-usage

        using (DocumentStore documentStore = new DocumentStore
        {
            Url = "http://locationOfRavenDbInstance:8083/"
        })
        {
            documentStore.Initialize();
            AddUserToDatabase(documentStore, "UserNameToAdd");
        }

        #endregion
    }

    #region raven-add-user
    public static void AddUserToDatabase(IDocumentStore documentStore, string username)
    {
        IDatabaseCommands systemCommands = documentStore
            .DatabaseCommands
            .ForSystemDatabase();
        WindowsAuthDocument windowsAuthDocument = GetWindowsAuthDocument(systemCommands);
        AddOrUpdateAuthUser(windowsAuthDocument, username, "<system>");

        RavenJObject ravenJObject = RavenJObject.FromObject(windowsAuthDocument);
        systemCommands.Put("Raven/Authorization/WindowsSettings", null, ravenJObject, new RavenJObject());
    }

    static WindowsAuthDocument GetWindowsAuthDocument(IDatabaseCommands systemCommands)
    {
        JsonDocument existing = systemCommands.Get("Raven/Authorization/WindowsSettings");
        if (existing == null)
        {
            return new WindowsAuthDocument();
        }
        return existing
            .DataAsJson
            .JsonDeserialization<WindowsAuthDocument>();
    }

    static void AddOrUpdateAuthUser(WindowsAuthDocument windowsAuthDocument, string identity, string tenantId)
    {
        WindowsAuthData windowsAuthForUser = windowsAuthDocument
            .RequiredUsers
            .FirstOrDefault(x => x.Name == identity);
        if (windowsAuthForUser == null)
        {
            windowsAuthForUser = new WindowsAuthData
            {
                Name = identity
            };
            windowsAuthDocument.RequiredUsers.Add(windowsAuthForUser);
        }
        windowsAuthForUser.Enabled = true;

        AddOrUpdateDataAccess(windowsAuthForUser, tenantId);
    }

    static void AddOrUpdateDataAccess(WindowsAuthData windowsAuthForUser, string tenantId)
    {
        ResourceAccess dataAccess = windowsAuthForUser
            .Databases
            .FirstOrDefault(x => x.TenantId == tenantId);
        if (dataAccess == null)
        {
            dataAccess = new ResourceAccess
            {
                TenantId = tenantId
            };
            windowsAuthForUser.Databases.Add(dataAccess);
        }
        dataAccess.ReadOnly = false;
        dataAccess.Admin = true;
    }

    class WindowsAuthDocument
    {
        public List<WindowsAuthData> RequiredGroups = new List<WindowsAuthData>();
        public List<WindowsAuthData> RequiredUsers = new List<WindowsAuthData>();
    }

    class WindowsAuthData
    {
        public string Name;
        public bool Enabled;
        public List<ResourceAccess> Databases = new List<ResourceAccess>();
    }
    #endregion
}

