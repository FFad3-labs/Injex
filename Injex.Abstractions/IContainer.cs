namespace Injex.Abstractions;

public interface IContainer
{
    object? GetService(Type serviceType);
}