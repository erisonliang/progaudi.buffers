using System;
using Shouldly;
using Xunit;

namespace ProGaudi.Buffers.Tests
{
    public class Ownership
    {
        [Fact]
        public void UnderlyingBufferIsNotOwnDispose()
        {
            using (var buffer = FixedLengthMemoryPool<object>.Shared.Rent())
            {
                var fixedLength = buffer.ShouldBeOfType<FixedLengthOwner<object>>();
                var underlying = fixedLength.GetUnderlyingBuffer(false);
                underlying.Dispose();
                Should.NotThrow(() => buffer.Memory);
                Should.NotThrow(() => fixedLength.Memory);
            }
        }

        [Fact]
        public void UnderlyingBufferIsOwnDispose()
        {
            using (var buffer = FixedLengthMemoryPool<object>.Shared.Rent())
            {
                var fixedLength = buffer.ShouldBeOfType<FixedLengthOwner<object>>();
                var underlying = fixedLength.GetUnderlyingBuffer(true);
                underlying.Dispose();
                Should.Throw<ObjectDisposedException>(() => { var _ = fixedLength.Memory; });
                Should.Throw<ObjectDisposedException>(() => { var _ = buffer.Memory; });
            }
        }
    }
}
