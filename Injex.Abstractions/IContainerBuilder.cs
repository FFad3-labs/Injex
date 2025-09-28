namespace Injex.Abstractions;

public interface IContainerBuilder : IList<ServiceDescriptor>
{
    IContainer Build();
}