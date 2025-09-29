using Injex.Abstractions;

namespace Injex;

internal class Container : IContainer
{
    private readonly List<ServiceDescriptor> _descriptors;

    public Container(IEnumerable<ServiceDescriptor> descriptors)
    {
        _descriptors = descriptors.ToList();
    }

    public object? GetService(Type serviceType)
        => Resolve(serviceType, new ResolutionContext());

    private object? Resolve(Type serviceType, ResolutionContext context)
    {
        var descriptor = _descriptors.LastOrDefault(d => d.ServiceType == serviceType);

        if (descriptor is null)
            return null;

        var impl = descriptor.ImplementationType;
        using var scope = context.CreateScope(impl);
        var factory = ServiceFactoryBuilder.BuildFor(impl);
        return factory((t) => Resolve(t, context));
    }
}