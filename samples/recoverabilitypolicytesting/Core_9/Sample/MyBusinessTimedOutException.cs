﻿using System;

public class MyBusinessTimedOutException : Exception
{
    public MyBusinessTimedOutException()
    {
    }

    public MyBusinessTimedOutException(string message)
        : base(message)
    {
    }

    public MyBusinessTimedOutException(string message, Exception inner)
        : base(message, inner)
    {
    }
}