using NServiceBus;

class ExtendableOptions
{
    void UseDeadLetterQueue()
    {
        #region UseDeadLetterQueue
        var options = new SendOptions();
        options.UseDeadLetterQueue();
        #endregion
    }
    void UseDeadLetterQueueFalse()
    {
        #region UseDeadLetterQueueFalse
        var options = new SendOptions();
        options.UseDeadLetterQueue(enable: false);
        #endregion
    }
    void UseJournalQueue()
    {
        #region UseJournalQueue
        var options = new SendOptions();
        options.UseJournalQueue();
        #endregion
    }
    void UseJournalQueueFalse()
    {
        #region UseJournalQueueFalse
        var options = new SendOptions();
        options.UseJournalQueue(enable: false);
        #endregion
    }
}
