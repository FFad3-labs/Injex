namespace Injex.Tests;

public class ResolutionContextTests
{
    [Fact]
    public void CreateScope_NullType_Throws()
    {
        //Arrange
        var ctx = new ResolutionContext();
        //Act
        var ex = Record.Exception(() => ctx.CreateScope(null!));
        //Assert
        Assert.NotNull(ex);
        Assert.IsType<ArgumentNullException>(ex);
    }

    [Fact]
    public void CreateScope_SameTypeTwice_ThrowsCircular_WithPath()
    {
        //Arrange
        var ctx = new ResolutionContext();
        using var _ = ctx.CreateScope(typeof(string));
        //Act
        var ex = Record.Exception(() => ctx.CreateScope(typeof(string)));
        //Assert
        Assert.NotNull(ex);
        Assert.IsType<InvalidOperationException>(ex);
    }

    [Fact]
    public void CreateScope_NestedDifferentTypes_NoThrow()
    {
        // Arrange
        var ctx = new ResolutionContext();
        using var _ = ctx.CreateScope(typeof(string));
        // Act
        var ex = Record.Exception(() =>
        {
            using (ctx.CreateScope(typeof(int)))
            {
            }
        });

        // Assert
        Assert.Null(ex);
    }

    [Fact]
    public void AfterDispose_CanEnterSameType()
    {
        // Arrange
        var ctx = new ResolutionContext();
        using (ctx.CreateScope(typeof(string)))
        {
        }

        // Act
        var ex = Record.Exception(() => ctx.CreateScope(typeof(string)));

        // Assert
        Assert.Null(ex);
    }

    [Fact]
    public void Dispose_IsIdempotent_NoThrowOnDoubleDispose()
    {
        // Arrange
        var ctx = new ResolutionContext();
        var scope = ctx.CreateScope(typeof(string));
        scope.Dispose();
        // Act
        var ex = Record.Exception(() => scope.Dispose());

        // Assert
        Assert.Null(ex);
    }

    [Fact]
    public void Dispose_OderMismatch_Throws()
    {
        // Arrange
        var ctx = new ResolutionContext();
        var outer = ctx.CreateScope(typeof(string));
        ctx.CreateScope(typeof(int));

        // Act
        var ex = Record.Exception(() => outer.Dispose());

        // Assert
        Assert.NotNull(ex);
        Assert.IsType<InvalidOperationException>(ex);
    }
}