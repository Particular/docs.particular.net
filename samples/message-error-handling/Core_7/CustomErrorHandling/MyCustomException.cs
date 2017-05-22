using System;
using System.Runtime.Serialization;

public class MyCustomException :
    Exception
{
    public MyCustomException()
    {
    }

    public MyCustomException(string message) :
        base(message)
    {
    }

    public MyCustomException(string message, Exception innerException) :
        base(message, innerException)
    {
    }

    protected MyCustomException(SerializationInfo info, StreamingContext context) :
        base(info, context)
    {
    }
}