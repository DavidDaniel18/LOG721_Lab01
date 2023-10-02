using System.Reflection;
using System.Reflection.Emit;
using Domain.ProtoTransit.Entities.Header;
using Domain.ProtoTransit.Seedwork;

namespace Domain.ProtoTransit;

public abstract partial class ProtoTransit
{
    private protected readonly ProtoHeader Header;
    
    private static readonly Dictionary<MessageTypesEnum, Func<ProtoTransit>> ProtoTransitFactory = InitializeFromEnumDescriptions();

    private byte[]? _bytes;

    private protected ProtoTransit(ProtoHeader header)
    {
        Header = header;
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