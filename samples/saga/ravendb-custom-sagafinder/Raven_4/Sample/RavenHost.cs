using System;
using System.Diagnostics;
using Raven.Client.Embedded;
using Raven.Client.UniqueConstraints;

#region ravenhost

class RavenHost :
    IDisposable
{
    public RavenHost()
    {
        documentStore = new EmbeddableDocumentStore
        {
            DataDirectory = "Data",
            UseEmbeddedHttpServer = true,
            DefaultDatabase = "NServiceBus",
            Configuration =
            {
                Port = 32076,
                PluginsDirectory = "Plugins",
                HostName = "localhost",
                Settings =
                {
                    { "Raven/ActiveBundles", "Unique Constraints" }
                },
                DefaultStorageTypeName = "esent"
            }
        };
        documentStore.RegisterListener(new UniqueConstraintsStoreListener());
        documentStore.Initialize();

        // since hosting a fake raven server in process need to remove it from the logging pipeline
        Trace.Listeners.Clear();
        Trace.Listeners.Add(new DefaultTraceListener());
        Console.WriteLine("Raven server started on http://localhost:32076/");
    }

    EmbeddableDocumentStore documentStore;

    public void Dispose()
    {
        documentStore?.Dispose();
    }
}
#endregion