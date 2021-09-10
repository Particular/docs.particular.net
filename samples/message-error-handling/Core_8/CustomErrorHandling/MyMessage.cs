using System;
using NServiceBus;

public class MyMessage :
    ICommand
{
    public Guid Id { get; set; }
    public bool ThrowCustomException { get; set; }
}