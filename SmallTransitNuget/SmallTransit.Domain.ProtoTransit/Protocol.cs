using System.Reflection;
using System.Reflection.Emit;
using SmallTransit.Abstractions.Monads;
using SmallTransit.Domain.ProtoTransit.Entities.Header;
using SmallTransit.Domain.ProtoTransit.Seedwork;
using SmallTransit.Domain.ProtoTransit.ValueObjects.Properties;

namespace SmallTransit.Domain.ProtoTransit;

internal abstract partial class Protocol
{
    private protected readonly ProtoHeader Header;

    internal static readonly Dictionary<MessageTypesEnum, Func<Protocol>> ProtoTransitFactory = InitializeFromEnumDescriptions();

    private readonly Dictionary<Type, ProtoProperty> _protoProperties = new();

    private byte[]? _bytes;

    private protected Protocol(MessageTypesEnum messageType)
    {
        Header = new ProtoHeader(messageType);
    }

    private protected void AddProperty<TProperty>() where TProperty : ProtoProperty
    {
        _protoProperties.Add(typeof(TProperty), ProtoProperty.Create(typeof(TProperty)));
    }

    internal Result TrySetProperty<TProperty>(byte[] value) where TProperty : ProtoProperty
    {
        return TryGetProperty<TProperty>().Bind(property =>
        {
            property.Bytes = value;

            return Header.TrySetValue(property.HeaderType, value);
        });
    }

    internal Result<ProtoProperty> TryGetProperty<TProperty>() where TProperty : ProtoProperty => TryGetProperty(typeof(TProperty));

    private Result<ProtoProperty> TryGetProperty(Type type)
    {
        return _protoProperties.TryGetValue(type, out var protoProperty)?
                Result.Success(protoProperty) :
                Result.Failure<ProtoProperty>($"Property {type.Name} not found.");
    }

    private protected IEnumerable<ProtoProperty> GetProperties()
    {
        return _protoProperties.Values;
    }

    private static Dictionary<MessageTypesEnum, Func<Protocol>> InitializeFromEnumDescriptions()
    {
        var protoTransitFactory = new Dictionary<MessageTypesEnum, Func<Protocol>>();

        foreach (MessageTypesEnum value in Enum.GetValues(typeof(MessageTypesEnum)))
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            dynamic attribute = fieldInfo.GetCustomAttribute(typeof(MessageTypeAttribute<>));

            protoTransitFactory[value] = GetProtoTransitILConstructor(attribute.MessageType);
        }

        return protoTransitFactory;

        Func<Protocol> GetProtoTransitILConstructor(Type type)
        {
            var method = new DynamicMethod("EmitActivate", type, null, true);

            var generator = method.GetILGenerator();

            var constructor = type.GetConstructor(Type.EmptyTypes);

            generator.Emit(OpCodes.Newobj, constructor);

            generator.Emit(OpCodes.Ret);

            var emitActivate = (Func<Protocol>)method.CreateDelegate(typeof(Func<Protocol>));

            return emitActivate;
        }
    }
}