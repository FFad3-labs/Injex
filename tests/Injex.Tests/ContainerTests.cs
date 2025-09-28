using Injex.Abstractions;
using Injex.Extensions;

namespace Injex.Tests;

public class ContainerTests
{
    internal interface IService
    {
    }

    internal class Service1 : IService
    {
    }
    
    internal class Service2 : IService
    {
    }
    
    internal class Service3
    {
        private readonly IService _inner;

        public Service3(IService inner)
        {
            _inner = inner;
        }
    }

    [Fact]
    public void Resolve_Should_Return_Null_When_NotRegistered()
    {
        //Arrange
        var builder = new ContainerBuilder();
        var container = builder.Build();
        //Act
        var svc = container.GetService<IService>();
        //Assert
        Assert.Null(svc);
    }

    [Fact]
    public void Resolve_Should_Return_Service_Instance()
    {
        //Arrange
        var builder = new ContainerBuilder();
        builder.Add(new ServiceDescriptor(typeof(IService), typeof(Service1)));
        var container = builder.Build();
        //Act
        var svc = container.GetService<IService>();
        //Assert
        Assert.NotNull(svc);
    }
    
    [Fact]
    public void Resolve_Should_Return_Last_Registered_Service()
    {
        //Arrange
        var builder = new ContainerBuilder();
        builder.Add(new ServiceDescriptor(typeof(IService), typeof(Service1)));
        builder.Add(new ServiceDescriptor(typeof(IService), typeof(Service2)));
        var container = builder.Build();
        //Act
        var svc = container.GetService<IService>();
        //Assert
        Assert.IsType<Service2>(svc);
    }
    
    [Fact]
    public void Resolve_Should_Return_Service_Collection()
    {
        //Arrange
        var builder = new ContainerBuilder();
        builder.Add(new ServiceDescriptor(typeof(IService), typeof(Service1)));
        builder.Add(new ServiceDescriptor(typeof(IService), typeof(Service2)));
        var container = builder.Build();
        //Act
        var svc = container.GetService<IEnumerable<IService>>();
        //Assert
        var list = Assert.IsAssignableFrom<IEnumerable<IService>>(svc).ToList();

        Assert.Collection(list,
            x => Assert.IsType<Service1>(x),
            x => Assert.IsType<Service2>(x));
    }
    
    [Fact]
    public void Resolve_Should_Return_Service_With_Dependencies()
    {
        //Arrange
        var builder = new ContainerBuilder();
        builder.Add(new ServiceDescriptor(typeof(IService), typeof(Service1)));
        builder.Add(new ServiceDescriptor(typeof(Service3), typeof(Service3)));
        var container = builder.Build();
        //Act
        var svc = container.GetService<Service3>();
        //Assert
        Assert.IsType<Service3>(svc);
    }
}