using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using NServiceBus.Logging;
using NServiceBus.MessageMutator;
#region ValidationMessageMutator
public class ValidationMessageMutator :   IMutateIncomingMessages, IMutateOutgoingMessages
{
    static ILog log = LogManager.GetLogger("ValidationMessageMutator");


    public void MutateOutgoing(MutateOutgoingMessagesContext context)
    {
        object messageInstance = context.MessageInstance;
        ValidateDataAnnotations(ref messageInstance);
        context.MessageInstance = messageInstance;
    }


    public object MutateIncoming(object message)
    {
        ValidateDataAnnotations(ref message);
        return message;
    }

    static void ValidateDataAnnotations(ref object message)
    {
        ValidationContext context = new ValidationContext(message, null, null);
        List<ValidationResult> results = new List<ValidationResult>();

        bool isValid = Validator.TryValidateObject(message, context, results, true);

        if (isValid)
        {
            log.Info("Validation succeeded for message: " + message);
            return;
        }

        StringBuilder errorMessage = new StringBuilder();
        string error = string.Format("Validation failed for message {0}, with the following error/s: " + Environment.NewLine,message);
        errorMessage.Append(error);

        foreach (ValidationResult validationResult in results)
        {
            errorMessage.Append(validationResult.ErrorMessage + Environment.NewLine);
        }

        log.Error(errorMessage.ToString());
        throw new Exception(errorMessage.ToString());
    }


}
#endregion