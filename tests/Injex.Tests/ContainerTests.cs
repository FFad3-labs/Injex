using Injex.Abstractions;
using Injex.Extensions;

namespace Injex.Tests;

public class ContainerTests
{
    #region TestTypes

    internal interface IS
    {
    }

    internal class S1 : IS
    {
    }

    internal class S2 : IS
    {
    }

    internal class S3
    {
        private readonly IS _inner;

        public S3(IS inner)
        {
            _inner = inner;
        }
    }

    internal interface IA
    {
    }

    internal interface IB
    {
    }

    internal class A : IA
    {
        public A(IB b)
        {
            
        }
    }

    internal class B : IB
    {
        public B(IA a)
        {
            
        }
    }

    #endregion

    [Fact]
    public void Resolve_ShouldReturnNullWhenNotRegistered()
    {
        //Arrange
        var builder = new ContainerBuilder();
        var container = builder.Build();
        //Act
        var svc = container.GetService<IS>();
        //Assert
        Assert.Null(svc);
    }

    [Fact]
    public void Resolve_ShouldReturnServiceInstance()
    {
        //Arrange
        var builder = new ContainerBuilder();
        builder.Add(new ServiceDescriptor(typeof(IS), typeof(S1)));
        var container = builder.Build();
        //Act
        var svc = container.GetService<IS>();
        //Assert
        Assert.NotNull(svc);
    }

    [Fact]
    public void Resolve_ShouldReturnLastRegisteredService()
    {
        //Arrange
        var builder = new ContainerBuilder();
        builder.Add(new ServiceDescriptor(typeof(IS), typeof(S1)));
        builder.Add(new ServiceDescriptor(typeof(IS), typeof(S2)));
        var container = builder.Build();
        //Act
        var svc = container.GetService<IS>();
        //Assert
        Assert.IsType<S2>(svc);
    }

    [Fact]
    public void Resolve_ShouldReturnServiceCollection()
    {
        //Arrange
        var builder = new ContainerBuilder();
        builder.Add(new ServiceDescriptor(typeof(IS), typeof(S1)));
        builder.Add(new ServiceDescriptor(typeof(IS), typeof(S2)));
        var container = builder.Build();
        //Act
        var svc = container.GetService<IEnumerable<IS>>();
        //Assert
        var list = Assert.IsAssignableFrom<IEnumerable<IS>>(svc).ToList();

        Assert.Collection(list,
            x => Assert.IsType<S1>(x),
            x => Assert.IsType<S2>(x));
    }

    [Fact]
    public void Resolve_ShouldReturnServiceWithDependencies()
    {
        //Arrange
        var builder = new ContainerBuilder();
        builder.Add(new ServiceDescriptor(typeof(IS), typeof(S1)));
        builder.Add(new ServiceDescriptor(typeof(S3), typeof(S3)));
        var container = builder.Build();
        //Act
        var svc = container.GetService<S3>();
        //Assert
        Assert.IsType<S3>(svc);
    }

    [Fact]
    public void Resolve_ShouldThrowOnCircularDependenciesWithProperMessage()
    {
        //Arrange
        var builder = new ContainerBuilder();
        builder.Add(new ServiceDescriptor(typeof(IA), typeof(A)));
        builder.Add(new ServiceDescriptor(typeof(IB), typeof(B)));
        var container = builder.Build();
        //Act
        var ex = Record.Exception(() => container.GetService<IB>());
        //Assert
        Assert.NotNull(ex);
        Assert.IsType<InvalidOperationException>(ex);
    }
}