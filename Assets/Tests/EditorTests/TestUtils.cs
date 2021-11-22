using NUnit.Framework;
using TheseusAndMinotaur.Game;
using UnityEngine;

namespace TheseusAndMinotaur.Tests
{
    public class TestUtils
    {
        [Test]
        public void CheckBoardToGlobalConversion()
        {
            var boardPosition = new Vector2Int(y: 5, x: 3);
            var globalPosition = boardPosition.GetGlobalPosition();
            Assert.That(globalPosition, Is.EqualTo(new Vector3(y: 5f, x: 3f, z: 0f)));
        }
    }
}