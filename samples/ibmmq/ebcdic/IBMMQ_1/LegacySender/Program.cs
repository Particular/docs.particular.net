using System.Buffers.Binary;
using System.Collections;
using System.Text;
using IBM.WMQ;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

var host = "localhost";
var port = 1414;
var channel = "DEV.ADMIN.SVRCONN";
var user = "admin";
var password = "passw0rd";

// EBCDIC International (Latin-1) code page
var ebcdic = Encoding.GetEncoding("IBM500");

var props = new Hashtable
{
    { MQC.TRANSPORT_PROPERTY, MQC.TRANSPORT_MQSERIES_MANAGED },
    { MQC.HOST_NAME_PROPERTY, host },
    { MQC.PORT_PROPERTY, port },
    { MQC.CHANNEL_PROPERTY, channel },
    { MQC.USER_ID_PROPERTY, user },
    { MQC.PASSWORD_PROPERTY, password },

};

#region CreateEBCDICMessage

using var qm = new MQQueueManager("QM1", props);
using var queue = qm.AccessQueue("DEV.RECEIVER", MQC.MQOO_OUTPUT);

Console.WriteLine("Sending a COBOL-like, fixed length EBCDIC text encoded message...");

var orderId = Guid.NewGuid();
var product = "Widget";
var quantity = Random.Shared.Next(1, 10);

var body = new byte[70];

// OrderId: 36 bytes EBCDIC
ebcdic.GetBytes(orderId.ToString(), body.AsSpan(0, 36));

// Product: 30 bytes EBCDIC, space-padded
var productSpan = body.AsSpan(36, 30);
productSpan.Fill(ebcdic.GetBytes(" ")[0]); // EBCDIC space = 0x40
ebcdic.GetBytes(product, productSpan);

// Quantity: 4 bytes big-endian int32
BinaryPrimitives.WriteInt32BigEndian(body.AsSpan(66, 4), quantity);

var msg = new MQMessage();
msg.CharacterSet = 500;// Set character set to IBM500 EBCDIC
msg.Format = MQC.MQFMT_NONE;
msg.Write(body);

queue.Put(msg);
#endregion

Console.WriteLine($"Sent EBCDIC PlaceOrder {{ OrderId = {orderId}, Product = {product}, Quantity = {quantity} }}");
Console.ReadKey();