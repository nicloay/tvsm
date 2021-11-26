using NUnit.Framework;
using TheseusAndMinotaur.Data;
using UnityEngine;

namespace TheseusAndMinotaur.Tests.Tests.EditorTests
{
    public class DirectionTest
    {
        [Test]
        public void TestCombineWithNoneDirection()
        {
            var direction = Direction.Left | Direction.None;
            Assert.That(direction, Is.EqualTo(Direction.Left));
        }

        [Test]
        public void TestIsBaseDirectionsExtension()
        {
            Assert.That(Direction.Left.IsBaseDirection(), Is.True);
            Assert.That(Direction.Right.IsBaseDirection(), Is.True);
            Assert.That(Direction.Up.IsBaseDirection(), Is.True);
            Assert.That(Direction.Down.IsBaseDirection(), Is.True);

            Assert.That((Direction.Left | Direction.Up).IsBaseDirection(), Is.False);
        }

        [Test]
        public void GetNeighbourTest()
        {
            var boardPosition = new Vector2Int(5, 3);
            Assert.That(boardPosition.GetNeighbour(Direction.Left), Is.EqualTo(new Vector2Int(4, 3)));
            Assert.That(boardPosition.GetNeighbour(Direction.Right), Is.EqualTo(new Vector2Int(6, 3)));
            Assert.That(boardPosition.GetNeighbour(Direction.Up), Is.EqualTo(new Vector2Int(5, 4)));
            Assert.That(boardPosition.GetNeighbour(Direction.Down), Is.EqualTo(new Vector2Int(5, 2)));
        }
    }
}