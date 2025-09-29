using Injex.Abstractions;

namespace Injex.Extensions;

public static class ContainerExtensions
{
    public static T? GetService<T>(this IContainer container)
        => (T?)container.GetService(typeof(T));

    public static T GetRequiredService<T>(this IContainer container) where T : notnull
        => GetService<T>(container) ??
           throw new InvalidOperationException($"No service for type '{typeof(T).FullName}' has been registered.");

    public static object GetRequiredService(this IContainer container, Type serviceType)
        => container.GetService(serviceType)
           ?? throw new InvalidOperationException($"No service for type '{serviceType.FullName}' has been registered.");
}