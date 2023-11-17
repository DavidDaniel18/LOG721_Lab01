namespace SmallTransit.Domain.ProtoTransit.Extensions;

public static class TypeExtensions
{
    public static string GetTypeName(this Type type)
    {
        if (!type.IsGenericType)
        {
            return type.Name;
        }

        var genericTypeName = type.GetGenericTypeDefinition().Name;

        // Remove the generic parameter count from the name
        genericTypeName = genericTypeName[..genericTypeName.IndexOf('`')];

        var genericArgs = string.Join(", ", type.GetGenericArguments().Select(GetTypeName));

        return $"{genericTypeName}<{genericArgs}>";
    }

    public static Type? FindMatchingType(this string typeName)
    {
        var allAssemblies = AppDomain.CurrentDomain.GetAssemblies();

        return allAssemblies.Select(assembly => assembly.GetTypes().FirstOrDefault(t => GetTypeName(t) == typeName)).FirstOrDefault(matchingType => matchingType != null);
    }
}