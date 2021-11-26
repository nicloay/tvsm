using NUnit.Framework;
using TheseusAndMinotaur.Utils;
using UnityEngine;

namespace TheseusAndMinotaur.Tests
{
    public class ArrayUtilsTest 
    {
        [Test]
        public void TestConversion()
        {
            Assert.That(13.Get2DCoordinates(5), Is.EqualTo(new Vector2Int(3,2)));
            Assert.That(14.Get2DCoordinates(5), Is.EqualTo(new Vector2Int(4,2)));
        }
    }
}