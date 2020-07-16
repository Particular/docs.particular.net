namespace CustomTimeoutManager
{
    using System;

    class PoisonMessageException : Exception
    {
        public PoisonMessageException(string message) : base(message)
        {
        }
    }
}