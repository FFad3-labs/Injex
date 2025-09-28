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
    {
        var descriptor = _descriptors.FirstOrDefault(d => d.ServiceType == serviceType);
        
        if(descriptor is null)
            return null;
        
        return Activator.CreateInstance(descriptor!.ImplementationType);
    }
}