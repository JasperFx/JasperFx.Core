namespace JasperFx.Core.Reflection;

public static class EnumerableTypeExtensions
{
    public static bool IsEnumerable(this Type type)
    {
        if (type.IsArray) return true;

        return type.IsGenericType && _enumerableTypes.Contains(type.GetGenericTypeDefinition());
    }

    public static Type DetermineElementType(this Type serviceType)
    {
        if (serviceType.IsArray)
        {
            return serviceType.GetElementType();
        }

        return serviceType.GetGenericArguments().First();
    }
        
    private static readonly List<Type> _enumerableTypes = new List<Type>
    {
        typeof (IEnumerable<>),
        typeof (IList<>),
        typeof (IReadOnlyList<>),
        typeof (List<>),
        typeof (ICollection<>),
        typeof (IReadOnlyCollection<>)
    };
}