
using Injex.Abstractions;

namespace Injex.Extensions;

public static class ContainerExtensions
{
    public static T? GetService<T>(this IContainer container)
        => (T?)container.GetService(typeof(T));
}