namespace Core6.Correlation
{
    using System.Threading.Tasks;
    using NServiceBus;

    class Usage
    {
        async Task Correlation(IPipelineContext context)
        {
            #region custom-correlationid
            var options = new SendOptions();

            options.SetCorrelationId("My custom correlation id");

            await context.Send(new MyRequest(),options)
                .ConfigureAwait(false);

            #endregion
        }

    }
}