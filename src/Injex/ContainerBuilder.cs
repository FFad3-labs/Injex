using Injex.Abstractions;

namespace Injex;

public class ContainerBuilder : List<ServiceDescriptor>, IContainerBuilder
{
    public IContainer Build() => new Container(this);
}