using NUnit.Framework;
using TheseusAndMinotaur.Data;
using TheseusAndMinotaur.Data.Deserializer;
using UnityEngine;

namespace TheseusAndMinotaur.Tests
{
    public class RawBoardTest
    {
        private static object[] _setValueSource =
        {
            new object[] { new Vector2Int(1, 4), Direction.Down },
            new object[] { new Vector2Int(3, 6), Direction.Up },
            new object[] { new Vector2Int(4, 7), Direction.Left },
            new object[] { new Vector2Int(5, 8), Direction.Right }
        };

        [Test]
        [TestCaseSource(nameof(_setValueSource))]
        public void TestSetValue(Vector2Int position, Direction value)
        {
            var board = new BoardDeserializer.RawBoard();
            board[position] = value;
            Assert.That(board[position], Is.EqualTo(value));
        }

        [Test]
        public void TestCombine()
        {
            var board = new BoardDeserializer.RawBoard();
            board.AddWall(3, 2, Direction.Left);
            board.AddWall(3, 2, Direction.Right);
            Assert.That(board[3, 2], Is.EqualTo(Direction.Left | Direction.Right));
        }
    }
}