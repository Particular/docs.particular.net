﻿using System;
using System.Threading.Tasks;
using System.IO;
using NServiceBus;
using NServiceBus.Attachments;
using NServiceBus.Testing;
using NServiceBus.Attachments.Testing;
using Xunit;

class TestingIncoming
{
    public void IncomingAttachment()
    {
        #region InjectAttachmentsInstance

        var context = new TestableMessageHandlerContext();
        var mockMessageAttachments = new MyMessageAttachments();
        context.InjectAttachmentsInstance(mockMessageAttachments);

        #endregion
    }

    #region CustomMockMessageAttachments

    public class CustomMockMessageAttachments : MockMessageAttachments
    {
        public override Task<byte[]> GetBytes()
        {
            GetBytesWasCalled = true;
            return Task.FromResult(new byte[] {5});
        }

        public bool GetBytesWasCalled { get; private set; }
    }

    #endregion

    #region TestOutgoingHandler

    public class Handler : IHandleMessages<MyMessage>
    {
        public async Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            var attachment = context.Attachments();
            var bytes = await attachment.GetBytes();
        }
    }

    #endregion

    #region TestIncoming

    [Fact]
    public async Task TestIncomingAttachment()
    {
        //Arrange
        var context = new TestableMessageHandlerContext();
        var handler = new Handler();
        var mockMessageAttachments = new CustomMockMessageAttachments();
        context.InjectAttachmentsInstance(mockMessageAttachments);

        //Act
        await handler.Handle(new MyMessage(), context);

        //Assert
        Assert.True(mockMessageAttachments.GetBytesWasCalled);
    }

    #endregion
}

class MyMessageAttachments : IMessageAttachments
{
    public Task CopyTo(string name, Stream target)
    {
        throw new NotImplementedException();
    }

    public Task CopyTo(Stream target)
    {
        throw new NotImplementedException();
    }

    public Task ProcessStream(string name, Func<Stream, Task> action)
    {
        throw new NotImplementedException();
    }

    public Task ProcessStream(Func<Stream, Task> action)
    {
        throw new NotImplementedException();
    }

    public Task ProcessStreams(Func<string, Stream, Task> action)
    {
        throw new NotImplementedException();
    }

    public Task<byte[]> GetBytes()
    {
        throw new NotImplementedException();
    }

    public Task<byte[]> GetBytes(string name)
    {
        throw new NotImplementedException();
    }

    public Task<Stream> GetStream()
    {
        throw new NotImplementedException();
    }

    public Task<Stream> GetStream(string name)
    {
        throw new NotImplementedException();
    }

    public Task CopyToForMessage(string messageId, string name, Stream target)
    {
        throw new NotImplementedException();
    }

    public Task CopyToForMessage(string messageId, Stream target)
    {
        throw new NotImplementedException();
    }

    public Task ProcessStreamForMessage(string messageId, string name, Func<Stream, Task> action)
    {
        throw new NotImplementedException();
    }

    public Task ProcessStreamForMessage(string messageId, Func<Stream, Task> action)
    {
        throw new NotImplementedException();
    }

    public Task ProcessStreamsForMessage(string messageId, Func<string, Stream, Task> action)
    {
        throw new NotImplementedException();
    }

    public Task<byte[]> GetBytesForMessage(string messageId)
    {
        throw new NotImplementedException();
    }

    public Task<byte[]> GetBytesForMessage(string messageId, string name)
    {
        throw new NotImplementedException();
    }

    public Task<Stream> GetStreamForMessage(string messageId)
    {
        throw new NotImplementedException();
    }

    public Task<Stream> GetStreamForMessage(string messageId, string name)
    {
        throw new NotImplementedException();
    }
}