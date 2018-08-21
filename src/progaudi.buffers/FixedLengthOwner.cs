using System;
using System.Buffers;

namespace ProGaudi.Buffers
{
    public sealed partial class FixedLengthOwner<T> : IMemoryOwner<T>
    {
        private T[] _array;
        private readonly int _size;

        public FixedLengthOwner(int size)
        {
            _array = ArrayPool<T>.Shared.Rent(size);
            _size = size;
        }

        public Memory<T> Memory
        {
            get
            {
                var array = _array;
                if (array == null)
                {
                    throw new ObjectDisposedException(nameof(FixedLengthOwner<T>));
                }

                return new Memory<T>(array, 0, _size);
            }
        }

        public IMemoryOwner<T> GetUnderlyingBuffer(bool own = false) => new UnderlyingOwner(this, own);

        public void Dispose()
        {
            var array = _array;
            if (array == null) return;

            _array = null;
            ArrayPool<T>.Shared.Return(array);
        }
    }
}