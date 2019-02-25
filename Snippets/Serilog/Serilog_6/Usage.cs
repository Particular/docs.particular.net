using System;
using System.Collections.Generic;
using NServiceBus.Logging;
using NServiceBus.Serilog;
using Serilog;

class Usage
{
    Usage()
    {
        #region SerilogInCode

        Log.Logger = new LoggerConfiguration()
            .WriteTo.File("log.txt")
            .CreateLogger();

        LogManager.Use<SerilogFactory>();

        #endregion
    }

    void Seq()
    {
        #region SerilogSeq

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Seq("http://localhost:5341")
            .MinimumLevel.Information()
            .CreateLogger();

        LogManager.Use<SerilogFactory>();

        #endregion
    }

    void ExceptionLogState(Exception exception)
    {
        #region ExceptionLogState

        var data = exception.Data;
        if (data.Contains("ExceptionLogState"))
        {
            var logState = (ExceptionLogState) data["ExceptionLogState"];
            string endpoint = logState.ProcessingEndpoint;
            string messageId = logState.MessageId;
            string messageType = logState.MessageType;
            string correlationId = logState.CorrelationId;
            string conversationId = logState.ConversationId;
            string handlerType = logState.HandlerType;
            IReadOnlyDictionary<string, string> headers = logState.Headers;
            object message = logState.Message;
        }

        #endregion
    }
}