using System.Reflection;

namespace Injex;

internal static class ServiceFactoryBuilder
{
    public static Func<Func<Type,object?>, object> BuildFor(Type implementationType)
    {
        var ctorInfo = GetConstructor(implementationType);
        var paramsTypes = ctorInfo.GetParameters().Select(x => x.ParameterType).ToArray();

        return resolve =>
        {
            var args = new object[paramsTypes.Length];
            for (var i = 0; i < paramsTypes.Length; i++)
            {
                args[i] = resolve(paramsTypes[i]) ??
                          throw new Exception($"Service factory builder returned null for {paramsTypes[i]}");
            }

            return ctorInfo.Invoke(args);
        };
    }

    private static ConstructorInfo GetConstructor(Type implementationType)
    {
        var ctor = implementationType
            .GetConstructors()
            .MaxBy(x => x.GetParameters().Length);

        if (ctor is null)
            throw new Exception($"No constructor found for {implementationType.Name}");

        return ctor;
    }
}