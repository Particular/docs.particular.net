namespace AzureFunctions_1;

using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;

#region azure-functions-program-builder
class ProgramSetup
{
    public static async Task Main(string[] args)
    {
        var builder = FunctionsApplication.CreateBuilder(args);
        builder.AddNServiceBusFunctions();

        var host = builder.Build();
        await host.RunAsync();
    }
}
#endregion
