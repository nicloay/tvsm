using NUnit.Framework;
using TheseusAndMinotaur.Data;
using TheseusAndMinotaur.Data.Deserializer;
using UnityEngine;

namespace TheseusAndMinotaur.Tests
{
    public class GeneratorTest
    {
        /// <summary>
        ///     Theseus Minotaur Exit column, as this parameter is mandatory for all mazes now
        /// </summary>
        private const string TMEMockColumn = ". \n" +
                                             ".T\n" +
                                             ". \n" +
                                             ".M\n" +
                                             ". \n" +
                                             ".E\n";

        [Test]
        public void TestSingleCellConversion()
        {
            var boardSrc =
                TMEMockColumn +
                "._\n" +
                "| ";

            var board = BoardDeserializer.DeserializeFrom(boardSrc);

            Assert.That(board[0, 0], Is.EqualTo(Direction.Left | Direction.Top));
        }

        [Test]
        public void TestVerticalLines()
        {
            var boardSrc =
                TMEMockColumn +
                "._\n" +
                "  \n" +
                "._\n";

            var board = BoardDeserializer.DeserializeFrom(boardSrc);
            Assert.That(board.GetWallsAtRow(1), Is.EquivalentTo(new[] { Direction.Vertical }));
            Assert.That(board.GetWallsAtRow(0), Is.EquivalentTo(new[] { Direction.Top }));
        }

        [Test]
        public void TestHorizontalLines()
        {
            var boardSrc =
                TMEMockColumn +
                "._.\n" +
                "| |\n";

            var board = BoardDeserializer.DeserializeFrom(boardSrc);
            Assert.That(board.GetWallsAtRow(0),
                Is.EquivalentTo(new[] { Direction.Horizontal | Direction.Top, Direction.Left }));
        }

        [Test]
        public void TestCenterSquare()
        {
            var boardSrc =
                ". . . .\n" +
                ".T. . .\n" +
                ". ._. .\n" +
                ". |M| .\n" +
                ". ._. .\n" +
                ". . .E.\n";

            var board = BoardDeserializer.DeserializeFrom(boardSrc);
            Assert.That(board.GetWallsAtRow(0),
                Is.EquivalentTo(new[] { Direction.None, Direction.Top, Direction.None }));
            Assert.That(board.GetWallsAtRow(1),
                Is.EquivalentTo(new[] { Direction.Right, Direction.All, Direction.Left }));
            Assert.That(board.GetWallsAtRow(2),
                Is.EquivalentTo(new[] { Direction.None, Direction.Down, Direction.None }));
        }

        [Test]
        public void TestMissedSymbols()
        {
            var boardSrc =
                TMEMockColumn +
                ". \n" +
                "\n" +
                ". ._.\n" +
                ". | | .\n" +
                ". ._\n";

            var board = BoardDeserializer.DeserializeFrom(boardSrc);

            Assert.That(board.GetWallsAtRow(0),
                Is.EquivalentTo(new[] { Direction.None, Direction.Top, Direction.None }));
            Assert.That(board.GetWallsAtRow(1),
                Is.EquivalentTo(new[] { Direction.Right, Direction.All, Direction.Left }));
            Assert.That(board.GetWallsAtRow(2),
                Is.EquivalentTo(new[] { Direction.None, Direction.Down, Direction.None }));
        }


        [Test]
        public void TestMinotaurTheseusExitPosition()
        {
            var boardSrc =
                ". . . .\n" +
                ". .E. .\n" +
                ". ._. .\n" +
                ". |T| .\n" +
                ". ._. .\n" +
                ".M. . .\n";

            var board = BoardDeserializer.DeserializeFrom(boardSrc);
            Assert.That(board.MinotaurStartPosition, Is.EqualTo(new Vector2Int(0, 0)));
            Assert.That(board.TheseusStartPosition, Is.EqualTo(new Vector2Int(1, 1)));
            Assert.That(board.Exit, Is.EqualTo(new Vector2Int(1, 2)));
        }

        [Test]
        public void TestMinotaurTheseusExitPosition2()
        {
            var boardSrc =
                ". . . .\n" +
                ".E. . .\n" +
                ". ._. .\n" +
                ". | |T.\n" +
                ". ._. .\n" +
                ". .M. .\n";

            var board = BoardDeserializer.DeserializeFrom(boardSrc);
            Assert.That(board.MinotaurStartPosition, Is.EqualTo(new Vector2Int(1, 0)));
            Assert.That(board.TheseusStartPosition, Is.EqualTo(new Vector2Int(2, 1)));
            Assert.That(board.Exit, Is.EqualTo(new Vector2Int(0, 2)));
        }

        [Test]
        public void TestNullStringException()
        {
            Assert.Throws<ParseMazeException>(() => BoardDeserializer.DeserializeFrom(null));
        }

        [Test]
        public void TestNoExitException()
        {
            var boardSrc =
                ". ._. .\n" +
                ". | |T.\n" +
                ". ._. .\n" +
                ". .M. .\n";

            Assert.Throws<ParseMazeException>(() => BoardDeserializer.DeserializeFrom(boardSrc));
        }
    }
}