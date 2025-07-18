using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus.MessageMutator;
#region ValidationMessageMutator

public class ValidationMessageMutator(ILogger<ValidationMessageMutator> logger) :
    IMutateIncomingMessages,
    IMutateOutgoingMessages
{

    public Task MutateOutgoing(MutateOutgoingMessageContext context)
    {
        ValidateDataAnnotations(context.OutgoingMessage);
        return Task.CompletedTask;
    }

    public Task MutateIncoming(MutateIncomingMessageContext context)
    {
        ValidateDataAnnotations(context.Message);
        return Task.CompletedTask;
    }

    void ValidateDataAnnotations(object message)
    {
        var context = new ValidationContext(message, null, null);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(message, context, results, true);

        if (isValid)
        {
            logger.LogInformation("Validation succeeded for message: {Message}", message);
            return;
        }

        var errorMessage = new StringBuilder();
        var error = $"Validation failed for message {message}, with the following error/s:";
        errorMessage.AppendLine(error);

        foreach (var validationResult in results)
        {
            errorMessage.AppendLine(validationResult.ErrorMessage);
        }

        logger.LogError(errorMessage.ToString());
        throw new Exception(errorMessage.ToString());
    }
}

#endregion