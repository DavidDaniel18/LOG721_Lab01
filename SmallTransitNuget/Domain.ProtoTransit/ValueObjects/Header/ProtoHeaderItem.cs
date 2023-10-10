using System.Reflection.Emit;

namespace Domain.ProtoTransit.ValueObjects.Header;

public abstract record ProtoHeaderItem(string Name, int HeaderLength, int Order = int.MaxValue)
{
    internal byte[]? HeaderValue { get; set; }

    private static readonly Dictionary<Type, Func<ProtoHeaderItem>> ProtoTransitItemFactory = InitializeFromEnumDescriptions();

    internal static ProtoHeaderItem Create(Type type)
    {
        if (!ProtoTransitItemFactory.TryGetValue(type, out var factory))
        {
            throw new InvalidOperationException($"No factory registered for type {type}");
        }

        return factory();
    }

    private static Dictionary<Type, Func<ProtoHeaderItem>> InitializeFromEnumDescriptions()
    {
        var protoTransitFactory = new Dictionary<Type, Func<ProtoHeaderItem>>();

        foreach (var type in GetAllTypesExceptCurrent())
        {
            protoTransitFactory[type] = GetProtoTransitItemILConstructor(type);
        }

        return protoTransitFactory;

        static Type[] GetAllTypesExceptCurrent()
        {
            var assembly = typeof(ProtoHeaderItem).Assembly;
            var namespaceToSearch = typeof(ProtoHeaderItem).Namespace;
            var currentType = typeof(ProtoHeaderItem);

            var types = assembly.GetTypes()
                .Where(t => t.Namespace == namespaceToSearch && t != currentType && t.DeclaringType is null)
                .ToArray();

            return types;
        }

        Func<ProtoHeaderItem> GetProtoTransitItemILConstructor(Type type)
        {
            var method = new DynamicMethod("EmitActivate", type, null, true);

            var generator = method.GetILGenerator();

            var constructor = type.GetConstructor(Type.EmptyTypes) ?? type.GetConstructor(new []{typeof(int)});

            generator.Emit(OpCodes.Newobj, constructor);

            generator.Emit(OpCodes.Ret);

            var emitActivate = (Func<ProtoHeaderItem>)method.CreateDelegate(typeof(Func<ProtoHeaderItem>));

            return emitActivate;
        }
    }
}