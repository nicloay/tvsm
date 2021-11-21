using NUnit.Framework;
using TheseusAndMinotaur.Data;

namespace TheseusAndMinotaur.Tests
{
    public class GeneratorTest
    {
        [Test]
        public void TestSingleCellConversion()
        {
            var boardSrc = 
                "._\n" +
                "| ";
            var board = BoardDeserializer.DeserializeFrom(boardSrc);
            Assert.That(board.Map, Is.EquivalentTo(new[,]
            {
                {Direction.Left | Direction.Top} 
            }));
        }
        
        [Test]
        public void TestVerticalLines()
        {
            var boardSrc = 
                "._\n" +
                "  \n" +
                "._\n";
                
            var board = BoardDeserializer.DeserializeFrom(boardSrc);
            Assert.That(board.Map, Is.EquivalentTo(new[,]
            {
                { Direction.Top}, // as last line miss horizontal wall it must be here
                { Direction.Vertical }  
            }));
        }
        
        [Test]
        public void TestHorizontalLines()
        {
            var boardSrc =
                "._.\n" +
                "| |\n";

            var board = BoardDeserializer.DeserializeFrom(boardSrc);
            Assert.That(board.Map, Is.EquivalentTo(new[,]
            {
                { Direction.Horizontal | Direction.Top, Direction.Left},
            }));
        }

        [Test]
        public void TestCenterSquare()
        {
            var boardSrc =
                ". . . .\n" +
                ". . . .\n" +
                ". ._. .\n" +
                ". | | .\n" +
                ". ._. .\n" +
                ". . . .\n";

            var board = BoardDeserializer.DeserializeFrom(boardSrc);
            Assert.That(board.Map, Is.EquivalentTo(new[,]
            {
                { Direction.None, Direction.Top, Direction.None},
                { Direction.Right, Direction.All, Direction.Left},
                { Direction.None , Direction.Down, Direction.None}
            }));
        }
        
        [Test]
        public void TestMissedSymbols()
        {
            var boardSrc =
                ". \n" +
                "\n" +
                ". ._.\n" +
                ". | | .\n" +
                ". ._\n";

                var board = BoardDeserializer.DeserializeFrom(boardSrc);
            Assert.That(board.Map, Is.EquivalentTo(new[,]
            {
                { Direction.None, Direction.Top, Direction.None},
                { Direction.Right, Direction.All, Direction.Left},
                { Direction.None , Direction.Down, Direction.None}
            }));
        }
    }
}