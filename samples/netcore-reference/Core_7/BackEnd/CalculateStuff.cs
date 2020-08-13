using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

internal class CalculateStuff : ICalculateStuff
{
    private readonly ILogger logger;

    public CalculateStuff(ILogger<CalculateStuff> logger)
    {
        this.logger = logger;
    }

    public Task Calculate(int number)
    {
        logger.LogInformation($"Calculating the value for {number}");

        return Task.CompletedTask;
    }
}
