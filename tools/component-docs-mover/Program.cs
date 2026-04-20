using Spectre.Console.Cli;

namespace component_docs_mover;

class Program
{
    static int Main(string[] args)
    {
        MoveDocsCommand.IsInteractiveLaunch = args.Length == 0;

        var app = new CommandApp<MoveDocsCommand>();
        app.Configure(config =>
        {
            config.SetApplicationName("component-docs-mover");
        });

        return app.Run(args);
    }
}