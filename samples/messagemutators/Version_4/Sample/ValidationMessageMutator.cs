using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using log4net;
using NServiceBus.MessageMutator;
#region ValidationMessageMutator
public class ValidationMessageMutator : IMessageMutator
{
    static ILog logger = LogManager.GetLogger("ValidationMessageMutator");

    public object MutateOutgoing(object message)
    {
        ValidateDataAnnotations(message);
        return message;
    }

    public object MutateIncoming(object message)
    {
        ValidateDataAnnotations(message);
        return message;
    }

    static void ValidateDataAnnotations(object message)
    {
        ValidationContext context = new ValidationContext(message, null, null);
        List<ValidationResult> results = new List<ValidationResult>();

        bool isValid = Validator.TryValidateObject(message, context, results, true);

        if (isValid)
        {
            logger.InfoFormat("Validation succeeded for message: {0}", message);
            return;
        }

        StringBuilder errorMessage = new StringBuilder();
        string error = string.Format("Validation failed for message {0}, with the following error/s: " + Environment.NewLine,message);
        errorMessage.Append(error);

        foreach (ValidationResult validationResult in results)
        {
            errorMessage.Append(validationResult.ErrorMessage + Environment.NewLine);
        }

        logger.Error(errorMessage.ToString());
        throw new Exception(errorMessage.ToString());
    }
}
#endregion