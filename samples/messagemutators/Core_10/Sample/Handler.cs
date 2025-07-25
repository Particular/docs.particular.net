using Microsoft.Extensions.Logging;


#region Handler

public class Handler(ILogger<Handler> logger) :
    IHandleMessages<CreateProductCommand>
{

    public Task Handle(CreateProductCommand message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received a CreateProductCommand message: {Message}", message);
        return Task.CompletedTask;
    }
}

#endregion