namespace Injex;

internal sealed class ResolutionContext
{
    private readonly HashSet<Type> _visited = new();
    private readonly Stack<Type> _stack = new();

    internal IDisposable CreateScope(Type type)
        => new ResolutionScope(this, type);

    private void Push(Type serviceType)
    {
        if (!_visited.Add(serviceType))
        {
            var cycle = string.Join(" -> ",
                _stack.Reverse().Select(t => t.FullName).Concat([serviceType.FullName!]));
            throw new InvalidOperationException($"Circular dependency detected: {cycle}");
        }
        _stack.Push(serviceType);
    }

    private void Pop(Type serviceType)
    {
        if (_stack.Count == 0 || _stack.Peek() != serviceType)
            throw new InvalidOperationException("Resolution stack corrupted (push/pop mismatch).");

        _stack.Pop();

        if (!_visited.Remove(serviceType))
            throw new InvalidOperationException($"Service type '{serviceType.FullName}' was not marked as visited.");
    }

    private sealed class ResolutionScope : IDisposable
    {
        private readonly ResolutionContext _ctx;
        private readonly Type _type;
        private bool _disposed;

        public ResolutionScope(ResolutionContext ctx, Type type)
        {
            _ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
            _type = type ?? throw new ArgumentNullException(nameof(type));
            _ctx.Push(type);
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            _ctx.Pop(_type);
        }
    }
}