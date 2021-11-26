using NUnit.Framework;
using TheseusAndMinotaur.WorldControllers;
using UnityEngine;

namespace TheseusAndMinotaur.Tests
{
    public class GeneralTests
    {
        [Test]
        public void CheckBoardToGlobalConversion()
        {
            var boardPosition = new Vector2Int(y: 5, x: 3);
            var globalPosition = boardPosition.GetWorldPosition();
            Assert.That(globalPosition, Is.EqualTo(new Vector3(y: 5f, x: 3f, z: 0f)));
        }
    }
}