using Shouldly;
using Xunit;

namespace ProGaudi.Buffers.Tests
{
    public class Length
    {
        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(1_000)]
        [InlineData(10_000)]
        [InlineData(100_000)]
        [InlineData(1_000_000)]
        [InlineData(10_000_000)]
        [InlineData(100_000_000)]
        [InlineData(1_000_000_000)]
        public void UsualRent(int length)
        {
            using (var buffer = FixedLengthMemoryPool<byte>.Shared.Rent(length))
            {
                buffer.Memory.Length.ShouldBe(length);
                var fixedOwner = buffer.ShouldBeOfType<FixedLengthOwner<byte>>();
                fixedOwner.GetUnderlyingBuffer().Memory.Length.ShouldBeGreaterThanOrEqualTo(length);
            }
        }

        [Fact]
        public void MinusOneProduce4K()
        {
            using (var buffer = FixedLengthMemoryPool<object>.Shared.Rent())
                buffer.Memory.Length.ShouldBe(4096);
        }
    }
}
