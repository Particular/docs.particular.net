using System;
using Raven.Client.Embedded;

class Program
{
    static void Main()
    {
        int port = 32076;
        using (EmbeddableDocumentStore documentStore = new EmbeddableDocumentStore
                                   {
                                       DataDirectory = "Data",
                                       UseEmbeddedHttpServer = true,
                                       Configuration =
                                       {
                                           PluginsDirectory = Environment.CurrentDirectory,
                                           Port = port,
                                           HostName = "localhost"
                                       },
                                       DefaultDatabase = "Samples.UoWWithChildContainers"
                                   })
        {
            documentStore.Initialize();
            Console.WriteLine("Raven server started on http://localhost:{0}/. Press enter to stop.", port);
            Console.ReadLine();
        }
    }
}