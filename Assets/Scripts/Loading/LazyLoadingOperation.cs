using System;

namespace Core.Loading
{
    public sealed class LazyLoadingOperation
    {
        private readonly Lazy<ILoadingOperation> Lazy;
        public ILoadingOperation Value => Lazy.Value;

        public LazyLoadingOperation(Func<ILoadingOperation> func)
        {
            Lazy = new Lazy<ILoadingOperation>(func);
        }

        public static implicit operator LazyLoadingOperation(Func<ILoadingOperation> func)
        {
            return new LazyLoadingOperation(func);
        }
    }
}