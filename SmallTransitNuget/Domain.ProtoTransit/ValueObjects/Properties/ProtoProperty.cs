using System.Reflection.Emit;

namespace Domain.ProtoTransit.ValueObjects.Properties;

internal abstract record ProtoProperty(Type HeaderType)
{
    internal byte[] Bytes = Array.Empty<byte>();

    private static readonly Dictionary<Type, Func<ProtoProperty>> ProtoPropertyFactory = InitializeFromEnumDescriptions();

    internal static ProtoProperty Create(Type type)
    {
        if (!ProtoPropertyFactory.TryGetValue(type, out var factory))
        {
            throw new InvalidOperationException($"No factory registered for type {type}");
        }

        return factory();
    }

    private static Dictionary<Type, Func<ProtoProperty>> InitializeFromEnumDescriptions()
    {
        var protoTransitFactory = new Dictionary<Type, Func<ProtoProperty>>();

        foreach (var type in GetAllTypesExceptCurrent())
        {
            protoTransitFactory[type] = GetProtoTransitItemILConstructor(type);
        }

        return protoTransitFactory;

        static Type[] GetAllTypesExceptCurrent()
        {
            var assembly = typeof(ProtoProperty).Assembly;
            var namespaceToSearch = typeof(ProtoProperty).Namespace;
            var currentType = typeof(ProtoProperty);

            var types = assembly.GetTypes()
                .Where(t => t.Namespace == namespaceToSearch && t != currentType)
                .ToArray();

            return types;
        }

        Func<ProtoProperty> GetProtoTransitItemILConstructor(Type type)
        {
            var method = new DynamicMethod("EmitActivate", type, null, true);

            var generator = method.GetILGenerator();

            generator.Emit(OpCodes.Newobj, type.GetConstructor(Type.EmptyTypes));

            generator.Emit(OpCodes.Ret);

            var emitActivate = (Func<ProtoProperty>)method.CreateDelegate(typeof(Func<ProtoProperty>));

            return emitActivate;
        }
    }
}