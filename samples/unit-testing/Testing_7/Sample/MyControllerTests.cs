﻿using System.Threading.Tasks;
using NServiceBus.Testing;
using NUnit.Framework;

[TestFixture]
public class MyControllerTests
{
    #region EndpointInstanceTest
    [Test]
    public async Task ShouldSendMessage()
    {
        var endpointInstance = new TestableEndpointInstance();
        var handler = new MyController(endpointInstance);

        await handler.HandleRequest();

        var sentMessages = endpointInstance.SentMessages;
        Assert.That(sentMessages.Length, Is.EqualTo(1));
        Assert.That(sentMessages[0].Message, Is.InstanceOf<MyMessage>());
    }
    #endregion
}