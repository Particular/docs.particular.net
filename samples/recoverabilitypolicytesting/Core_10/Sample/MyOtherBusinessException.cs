using System;

public class MyOtherBusinessException : Exception
{
    public MyOtherBusinessException()
    {
    }

    public MyOtherBusinessException(string message)
        : base(message)
    {
    }

    public MyOtherBusinessException(string message, Exception inner)
        : base(message, inner)
    {
    }
}