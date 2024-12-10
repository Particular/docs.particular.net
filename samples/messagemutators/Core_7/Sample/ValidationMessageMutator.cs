using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using NServiceBus.Logging;
using NServiceBus.MessageMutator;
#region ValidationMessageMutator

public class ValidationMessageMutator :
    IMutateIncomingMessages,
    IMutateOutgoingMessages
{
    static ILog log = LogManager.GetLogger("ValidationMessageMutator");

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

    static void ValidateDataAnnotations(object message)
    {
        var context = new ValidationContext(message, null, null);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(message, context, results, true);

        if (isValid)
        {
            log.Info($"Validation succeeded for message: {message}");
            return;
        }

        var errorMessage = new StringBuilder();
        var error = $"Validation failed for message {message}, with the following error/s:";
        errorMessage.AppendLine(error);

        foreach (var validationResult in results)
        {
            errorMessage.AppendLine(validationResult.ErrorMessage);
        }

        log.Error(errorMessage.ToString());
        throw new Exception(errorMessage.ToString());
    }
}

#endregion