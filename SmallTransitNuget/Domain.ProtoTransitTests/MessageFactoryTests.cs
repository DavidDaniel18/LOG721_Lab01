using Domain.Common.Monads;
using Domain.ProtoTransit;
using Domain.ProtoTransit.Entities.Messages.Core;
using Domain.ProtoTransit.Entities.Messages.Data;
using Domain.ProtoTransit.ValueObjects.Properties;
using MessagePack;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Domain.ProtoTransitTests
{
    [TestClass()]
    public class MessageFactoryTests
    {
        [TestMethod()]
        public void PublishProtocol()
        {
            var message = MessageFactory.Create(MessageTypesEnum.Publish);

            List<int> ints = new();

            for (int i = 0; i < 1000; i++)
            {
                ints.Add(i);   
            }

            var payload = new TestHeavyPayload()
            {
                Message = "John Doe",
                DateTime = DateTime.UtcNow,
                Id = Guid.NewGuid(),
                Ints = ints
            };

            var options = MessagePackSerializerOptions.Standard.WithResolver(MessagePack.Resolvers.ContractlessStandardResolver.Instance);

            var routingKey = MessagePackSerializer.Serialize("Key.Test");
            var payloadType = MessagePackSerializer.Serialize(nameof(TestHeavyPayload));
            var serializedPayload = MessagePackSerializer.Serialize(payload, options);

            Result.From(
                    message.TrySetProperty<RoutingKey>(routingKey), 
                    message.TrySetProperty<PayloadType>(payloadType), 
                    message.TrySetProperty<Payload>(serializedPayload))
                .ThrowIfException();


            Assert.IsTrue(message is Publish);

            var bytesResult = message.GetBytes();

            Assert.IsTrue(bytesResult.IsSuccess());
            Assert.IsTrue(bytesResult.Content!.Length > 0);

            var conversionResult = Protocol.TryParseMessage(bytesResult.Content!);

            Assert.IsTrue(conversionResult.IsSuccess());
            Assert.IsTrue(conversionResult.Content.Protocol is Publish);
            Assert.IsTrue(conversionResult.Content.Reminder.Length.Equals(0));

            var newBytesResult = conversionResult.Content.Protocol.GetBytes();

            Assert.IsTrue(newBytesResult.IsSuccess());
            Assert.IsTrue(conversionResult.Content.Reminder.Length == 0);

            var deserializedProtocol = conversionResult.Content.Protocol;

            var routingKeyResult = deserializedProtocol.TryGetProperty<RoutingKey>();
            var payloadTypeResult = deserializedProtocol.TryGetProperty<PayloadType>();
            var payloadResult = deserializedProtocol.TryGetProperty<Payload>();

            Result.From(
                    routingKeyResult,
                    payloadTypeResult,
                    payloadResult)
                .ThrowIfException();

            var newRoutingKey = MessagePackSerializer.Deserialize<string>(routingKeyResult.Content!.Bytes);
            var newPayloadType = MessagePackSerializer.Deserialize<string>(payloadTypeResult.Content!.Bytes);
            var newPayload = MessagePackSerializer.Deserialize<TestHeavyPayload>(payloadResult.Content!.Bytes, options);

            Assert.IsTrue(newRoutingKey.Equals("Key.Test"));
            Assert.IsTrue(newPayloadType.Equals(nameof(TestHeavyPayload)));
            Assert.IsTrue(newPayload.Equals(payload));
        }

        [TestMethod()]
        public void MultiplePublishProtocol()
        {
            var message = MessageFactory.Create(MessageTypesEnum.Publish);
            var message2 = MessageFactory.Create(MessageTypesEnum.Publish);

            var payload = new TestPayload()
            {
                Message = "John Doe",
                DateTime = DateTime.UtcNow,
                Id = Guid.NewGuid()

            };

            var options = MessagePackSerializerOptions.Standard.WithResolver(MessagePack.Resolvers.ContractlessStandardResolver.Instance);

            var routingKey = MessagePackSerializer.Serialize("Key.Test");
            var payloadType = MessagePackSerializer.Serialize(nameof(TestPayload));
            var serializedPayload = MessagePackSerializer.Serialize(payload, options);

            Result.From(
                    message.TrySetProperty<RoutingKey>(routingKey),
                    message.TrySetProperty<PayloadType>(payloadType),
                    message.TrySetProperty<Payload>(serializedPayload),
                    message2.TrySetProperty<RoutingKey>(routingKey),
                    message2.TrySetProperty<PayloadType>(payloadType),
                    message2.TrySetProperty<Payload>(serializedPayload))
                .ThrowIfException();


            Assert.IsTrue(message is Publish);

            var bytesResult = message.GetBytes();
            var bytesResult2 = message2.GetBytes();

            Assert.IsTrue(bytesResult.IsSuccess());
            Assert.IsTrue(bytesResult.Content!.Length > 0);

            Assert.IsTrue(bytesResult2.IsSuccess());
            Assert.IsTrue(bytesResult2.Content!.Length > 0);

            var resultingBytes = bytesResult.Content.Concat(bytesResult2.Content).ToArray();

            var conversionResult = Protocol.TryParseMessage(resultingBytes);

            Assert.IsTrue(conversionResult.IsSuccess());
            Assert.IsTrue(conversionResult.Content.Protocol is Publish);

            var newBytesResult = conversionResult.Content.Protocol.GetBytes();

            Assert.IsTrue(newBytesResult.IsSuccess());
            Assert.IsTrue(conversionResult.Content.Reminder.Length == bytesResult2.Content.Length);

            conversionResult = Protocol.TryParseMessage(conversionResult.Content.Reminder);

            var deserializedProtocol = conversionResult.Content.Protocol;

            var routingKeyResult = deserializedProtocol.TryGetProperty<RoutingKey>();
            var payloadTypeResult = deserializedProtocol.TryGetProperty<PayloadType>();
            var payloadResult = deserializedProtocol.TryGetProperty<Payload>();

            Result.From(
                    routingKeyResult,
                    payloadTypeResult,
                    payloadResult)
                .ThrowIfException();

            var newRoutingKey = MessagePackSerializer.Deserialize<string>(routingKeyResult.Content!.Bytes);
            var newPayloadType = MessagePackSerializer.Deserialize<string>(payloadTypeResult.Content!.Bytes);
            var newPayload = MessagePackSerializer.Deserialize<TestPayload>(payloadResult.Content!.Bytes, options);

            Assert.IsTrue(newRoutingKey.Equals("Key.Test"));
            Assert.IsTrue(newPayloadType.Equals(nameof(TestPayload)));
            Assert.IsTrue(newPayload.Equals(payload));
        }

        [TestMethod()]
        public void SubscribeProtocol()
        {
            var message = MessageFactory.Create(MessageTypesEnum.Subscribe);

            var options = MessagePackSerializerOptions.Standard.WithResolver(MessagePack.Resolvers.ContractlessStandardResolver.Instance);

            var routingKey = MessagePackSerializer.Serialize("Key.Test");
            var payloadType = MessagePackSerializer.Serialize(nameof(TestPayload));
            var queueName = MessagePackSerializer.Serialize("queueName");

            Result.From(
                    message.TrySetProperty<RoutingKey>(routingKey),
                    message.TrySetProperty<PayloadType>(payloadType),
                    message.TrySetProperty<QueueName>(queueName))
                .ThrowIfException();


            Assert.IsTrue(message is Subscribe);

            var bytesResult = message.GetBytes();

            Assert.IsTrue(bytesResult.IsSuccess());
            Assert.IsTrue(bytesResult.Content!.Length > 0);

            var conversionResult = Protocol.TryParseMessage(bytesResult.Content!);

            Assert.IsTrue(conversionResult.IsSuccess());
            Assert.IsTrue(conversionResult.Content.Protocol is Subscribe);
            Assert.IsTrue(conversionResult.Content.Reminder.Length.Equals(0));

            var newBytesResult = conversionResult.Content.Protocol.GetBytes();

            Assert.IsTrue(newBytesResult.IsSuccess());
            Assert.IsTrue(conversionResult.Content.Reminder.Length == 0);

            var deserializedProtocol = conversionResult.Content.Protocol;

            var routingKeyResult = deserializedProtocol.TryGetProperty<RoutingKey>();
            var payloadTypeResult = deserializedProtocol.TryGetProperty<PayloadType>();
            var queueNameResult = deserializedProtocol.TryGetProperty<QueueName>();

            Result.From(
                    routingKeyResult,
                    payloadTypeResult,
                    queueNameResult)
                .ThrowIfException();

            var newRoutingKey = MessagePackSerializer.Deserialize<string>(routingKeyResult.Content!.Bytes);
            var newPayloadType = MessagePackSerializer.Deserialize<string>(payloadTypeResult.Content!.Bytes);
            var newQueueName = MessagePackSerializer.Deserialize<string>(queueNameResult.Content!.Bytes, options);

            Assert.IsTrue(newRoutingKey.Equals("Key.Test"));
            Assert.IsTrue(newPayloadType.Equals(nameof(TestPayload)));
            Assert.IsTrue(newQueueName.Equals("queueName"));
        }

        [TestMethod()]
        public void PushProtocol()
        {
            var message = MessageFactory.Create(MessageTypesEnum.Push);

            var payload = new TestPayload()
            {
                Message = "John Doe",
                DateTime = DateTime.UtcNow,
                Id = Guid.NewGuid()

            };

            var options = MessagePackSerializerOptions.Standard.WithResolver(MessagePack.Resolvers.ContractlessStandardResolver.Instance);

            var serializedPayload = MessagePackSerializer.Serialize(payload, options);

            Result.From(
                    message.TrySetProperty<Payload>(serializedPayload))
                .ThrowIfException();


            Assert.IsTrue(message is Push);

            var bytesResult = message.GetBytes();

            Assert.IsTrue(bytesResult.IsSuccess());
            Assert.IsTrue(bytesResult.Content!.Length > 0);

            var conversionResult = Protocol.TryParseMessage(bytesResult.Content!);

            Assert.IsTrue(conversionResult.IsSuccess());
            Assert.IsTrue(conversionResult.Content.Protocol is Push);
            Assert.IsTrue(conversionResult.Content.Reminder.Length.Equals(0));

            var newBytesResult = conversionResult.Content.Protocol.GetBytes();

            Assert.IsTrue(newBytesResult.IsSuccess());
            Assert.IsTrue(conversionResult.Content.Reminder.Length == 0);

            var deserializedProtocol = conversionResult.Content.Protocol;

            var payloadResult = deserializedProtocol.TryGetProperty<Payload>();

            Result.From(
                    payloadResult)
                .ThrowIfException();

            var newPayload = MessagePackSerializer.Deserialize<TestPayload>(payloadResult.Content!.Bytes, options);

            Assert.IsTrue(newPayload.Equals(payload));
        }

        [TestMethod()]
        public void SendProtocol()
        {
            var message = MessageFactory.Create(MessageTypesEnum.Send);

            var payload = new TestPayload()
            {
                Message = "John Doe",
                DateTime = DateTime.UtcNow,
                Id = Guid.NewGuid()
            };

            var options = MessagePackSerializerOptions.Standard.WithResolver(MessagePack.Resolvers.ContractlessStandardResolver.Instance);

            var senderId = MessagePackSerializer.Serialize("Client1");
            var payloadType = MessagePackSerializer.Serialize(nameof(TestPayload));
            var serializedPayload = MessagePackSerializer.Serialize(payload, options);

            Result.From(
                    message.TrySetProperty<SenderId>(senderId),
                    message.TrySetProperty<PayloadType>(payloadType),
                    message.TrySetProperty<Payload>(serializedPayload))
                .ThrowIfException();


            Assert.IsTrue(message is Send);

            var bytesResult = message.GetBytes();

            Assert.IsTrue(bytesResult.IsSuccess());
            Assert.IsTrue(bytesResult.Content!.Length > 0);

            var conversionResult = Protocol.TryParseMessage(bytesResult.Content!);

            Assert.IsTrue(conversionResult.IsSuccess());
            Assert.IsTrue(conversionResult.Content.Protocol is Send);
            Assert.IsTrue(conversionResult.Content.Reminder.Length.Equals(0));

            var newBytesResult = conversionResult.Content.Protocol.GetBytes();

            Assert.IsTrue(newBytesResult.IsSuccess());
            Assert.IsTrue(conversionResult.Content.Reminder.Length == 0);

            var deserializedProtocol = conversionResult.Content.Protocol;

            var senderIdResult = deserializedProtocol.TryGetProperty<SenderId>();
            var payloadTypeResult = deserializedProtocol.TryGetProperty<PayloadType>();
            var payloadResult = deserializedProtocol.TryGetProperty<Payload>();

            Result.From(
                    senderIdResult,
                    payloadTypeResult,
                    payloadResult)
                .ThrowIfException();

            var newSenderId = MessagePackSerializer.Deserialize<string>(senderIdResult.Content!.Bytes);
            var newPayloadType = MessagePackSerializer.Deserialize<string>(payloadTypeResult.Content!.Bytes);
            var newPayload = MessagePackSerializer.Deserialize<TestPayload>(payloadResult.Content!.Bytes, options);

            Assert.IsTrue(newSenderId.Equals("Client1"));
            Assert.IsTrue(newPayloadType.Equals(nameof(TestPayload)));
            Assert.IsTrue(newPayload.Equals(payload));
        }

        [TestMethod()]
        public void PayloadResponseProtocol()
        {
            var message = MessageFactory.Create(MessageTypesEnum.PayloadResponse);

            var payload = new TestPayload()
            {
                Message = "John Doe",
                DateTime = DateTime.UtcNow,
                Id = Guid.NewGuid()

            };

            var options = MessagePackSerializerOptions.Standard.WithResolver(MessagePack.Resolvers.ContractlessStandardResolver.Instance);

            var payloadType = MessagePackSerializer.Serialize(nameof(TestPayload));
            var serializedPayload = MessagePackSerializer.Serialize(payload, options);

            Result.From(
                    message.TrySetProperty<PayloadType>(payloadType),
                    message.TrySetProperty<Payload>(serializedPayload))
                .ThrowIfException();


            Assert.IsTrue(message is PayloadResponse);

            var bytesResult = message.GetBytes();

            Assert.IsTrue(bytesResult.IsSuccess());
            Assert.IsTrue(bytesResult.Content!.Length > 0);

            var conversionResult = Protocol.TryParseMessage(bytesResult.Content!);

            Assert.IsTrue(conversionResult.IsSuccess());
            Assert.IsTrue(conversionResult.Content.Protocol is PayloadResponse);
            Assert.IsTrue(conversionResult.Content.Reminder.Length.Equals(0));

            var newBytesResult = conversionResult.Content.Protocol.GetBytes();

            Assert.IsTrue(newBytesResult.IsSuccess());
            Assert.IsTrue(conversionResult.Content.Reminder.Length == 0);

            var deserializedProtocol = conversionResult.Content.Protocol;

            var payloadTypeResult = deserializedProtocol.TryGetProperty<PayloadType>();
            var payloadResult = deserializedProtocol.TryGetProperty<Payload>();

            Result.From(
                    payloadTypeResult,
                    payloadResult)
                .ThrowIfException();

            var newPayloadType = MessagePackSerializer.Deserialize<string>(payloadTypeResult.Content!.Bytes);
            var newPayload = MessagePackSerializer.Deserialize<TestPayload>(payloadResult.Content!.Bytes, options);

            Assert.IsTrue(newPayloadType.Equals(nameof(TestPayload)));
            Assert.IsTrue(newPayload.Equals(payload));
        }

        [TestMethod()]
        [DataRow(MessageTypesEnum.Ack, typeof(Ack))]
        [DataRow(MessageTypesEnum.Close, typeof(Close))]
        [DataRow(MessageTypesEnum.Nack, typeof(Nack))]
        [DataRow(MessageTypesEnum.HandShake, typeof(HandShake))]
        public void CoreProtocolTest(MessageTypesEnum messageType, Type t)
        {
            var message = MessageFactory.Create(messageType);

            Assert.IsTrue(message.GetType() == t);

            var bytesResult = message.GetBytes();

            Assert.IsTrue(bytesResult.IsSuccess());
            Assert.IsTrue(bytesResult.Content!.Length > 0);

            var conversionResult = Protocol.TryParseMessage(bytesResult.Content!);

            Assert.IsTrue(conversionResult.IsSuccess());
            Assert.IsTrue(conversionResult.Content.Protocol.GetType() == t);
            Assert.IsTrue(conversionResult.Content.Reminder.Length.Equals(0));

            var newBytesResult = conversionResult.Content.Protocol.GetBytes();

            Assert.IsTrue(newBytesResult.IsSuccess());
            Assert.IsTrue(conversionResult.Content.Reminder.Length == 0);
        }

        public class TestHeavyPayload : IEquatable<TestHeavyPayload>
        {
            public Guid Id { get; set; }

            public DateTime DateTime { get; set; }

            public string Message { get; set; }

            public List<int> Ints { get; set; }

            public bool Equals(TestHeavyPayload? other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return Id.Equals(other.Id) && DateTime.Equals(other.DateTime) && Message == other.Message;
            }

            public override bool Equals(object? obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((TestHeavyPayload)obj);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Id, DateTime, Message);
            }
        }

        public class TestPayload : IEquatable<TestPayload>
        {
            public Guid Id { get; set; }

            public DateTime DateTime { get; set; }

            public string Message { get; set; }

            public bool Equals(TestPayload? other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return Id.Equals(other.Id) && DateTime.Equals(other.DateTime) && Message == other.Message;
            }

            public override bool Equals(object? obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((TestPayload)obj);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Id, DateTime, Message);
            }
        }

    }
}