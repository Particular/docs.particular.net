namespace AzureFunctions_1;

using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;

class ProgramSetup
{
    public static async Task Main(string[] args)
    {
        #region azure-functions-program-builder
        var builder = FunctionsApplication.CreateBuilder(args);
        builder.AddNServiceBusFunctions();

        var host = builder.Build();
        await host.RunAsync();
        #endregion
    }
}
