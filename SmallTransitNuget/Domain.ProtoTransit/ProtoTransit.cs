using System.Reflection;
using System.Reflection.Emit;
using Domain.Common;
using Domain.ProtoTransit.Entities.Header;
using Domain.ProtoTransit.Seedwork;
using Domain.ProtoTransit.ValueObjects.Properties;

namespace Domain.ProtoTransit;

public abstract partial class ProtoTransit
{
    private protected readonly ProtoHeader Header;

    private static readonly Dictionary<MessageTypesEnum, Func<ProtoTransit>> ProtoTransitFactory = InitializeFromEnumDescriptions();

    private readonly Dictionary<Type, ProtoProperty> _protoProperties = new();

    private byte[]? _bytes;

    private protected ProtoTransit(MessageTypesEnum messageType)
    {
        Header = new ProtoHeader(messageType);
    }

    private protected void AddProperty<TProperty>() where TProperty : ProtoProperty
    {
        _protoProperties.Add(typeof(TProperty), ProtoProperty.Create(typeof(TProperty)));
    }

    internal Result TrySetProperty<TProperty>(byte[] value) where TProperty : ProtoProperty
    {
        var propertyResult = TryGetProperty(typeof(TProperty));

        if (propertyResult.IsFailure()) return Result.FromFailure(propertyResult);

        propertyResult.Content!.Bytes = value;

        Header.TrySetValue(propertyResult.Content!.HeaderType, value);

        return Result.Success();
    }

    private Result<ProtoProperty> TryGetProperty(Type type)
    {
        if (_protoProperties.TryGetValue(type, out var protoProperty))
        {
            return Result.Success(protoProperty);
        }

        return Result.Failure<ProtoProperty>($"Property {type.Name} not found.");
    }

    private protected IEnumerable<ProtoProperty> GetProperties()
    {
        return _protoProperties.Values;
    }

    private static Dictionary<MessageTypesEnum, Func<ProtoTransit>> InitializeFromEnumDescriptions()
    {
        var protoTransitFactory = new Dictionary<MessageTypesEnum, Func<ProtoTransit>>();

        foreach (MessageTypesEnum value in Enum.GetValues(typeof(MessageTypesEnum)))
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            dynamic attribute = fieldInfo.GetCustomAttribute(typeof(MessageTypeAttribute<>));

            protoTransitFactory[value] = GetProtoTransitILConstructor(attribute.MessageType);
        }

        return protoTransitFactory;

        Func<ProtoTransit> GetProtoTransitILConstructor(Type type)
        {
            var method = new DynamicMethod("EmitActivate", type, null, true);

            var generator = method.GetILGenerator();

            generator.Emit(OpCodes.Newobj, type.GetConstructor(Type.EmptyTypes));

            generator.Emit(OpCodes.Ret);

            var emitActivate = (Func<ProtoTransit>)method.CreateDelegate(typeof(Func<ProtoTransit>));

            return emitActivate;
        }
    }
}