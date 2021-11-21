using NUnit.Framework;
using TheseusAndMinotaur.Data;

namespace TheseusAndMinotaur.Tests
{
    public class DirectionTest
    {
        [Test]
        public void TestCombineWithNoneDirection()
        {
            var direction = Direction.Left | Direction.None;
            Assert.That(direction, Is.EqualTo(Direction.Left));
        }
    }
}