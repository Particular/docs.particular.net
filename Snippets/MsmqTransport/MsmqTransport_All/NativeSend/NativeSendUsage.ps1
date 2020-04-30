# startcode msmq-nativesend-powershell-usage
SendMessage -QueuePath 'MachineName\private$\QueueName' `
            -MessageBody:  '{\"Property\":\"PropertyValue\"}' `
            -Headers  @{ 'NServiceBus.EnclosedMessageTypes' = 'MyNamespace.MyMessage'; }
# endcode