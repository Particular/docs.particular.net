using Microsoft.Azure.WebJobs;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace AzureFunctions.ASBTrigger.FunctionsHostBuilder
{
    public interface IContextPublisher
    {
        Task Do(ExecutionContext executionContext, ILogger logger);
    }
}