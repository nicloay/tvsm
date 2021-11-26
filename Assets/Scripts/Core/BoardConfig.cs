using TheseusAndMinotaur.Data.Converter;
using TheseusAndMinotaur.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace TheseusAndMinotaur.Data
{
    /// <summary>
    ///     Data class which represent original board configuration.
    /// </summary>
    public class BoardConfig
    {
        public readonly Vector2Int Exit;
        public readonly Vector2Int MinotaurStartPosition;
        public readonly Vector2Int TheseusStartPosition;
        internal readonly Direction[,] WallMap;


        public BoardConfig(Direction[,] wallMap, Vector2Int theseusStartPosition, Vector2Int minotaurStartPosition,
            Vector2Int exit)
        {
            Assert.IsTrue(theseusStartPosition != minotaurStartPosition,
                "theseus and minotaur position must be different");
            Assert.IsTrue(theseusStartPosition != exit, "theseus and exit position must be different");
            Assert.IsTrue(minotaurStartPosition != exit, "minotaur and exit position must be different");

            TheseusStartPosition = theseusStartPosition;
            MinotaurStartPosition = minotaurStartPosition;
            WallMap = wallMap;
            Exit = exit;
        }

        /// <summary>
        ///     Get wall directions at specified position
        ///     pivot point: BottomLeft
        /// </summary>
        public Direction this[int y, int x] => WallMap[y, x];

        /// <summary>
        ///     Get wall directions at specified position
        ///     pivot point: BottomLeft
        /// </summary>
        public Direction this[Vector2Int boardPosition] => this[boardPosition.y, boardPosition.x];

        public int Height => WallMap.GetLength(0);
        public int Width => WallMap.GetLength(1);

        public Vector2Int GridSize => new(Width, Height);

        public Direction[] GetWallsAtRow(int rowId)
        {
            var result = new Direction[Width];
            for (var x = 0; x < Width; x++)
            {
                result[x] = WallMap[rowId, x];
            }

            return result;
        }
        
        /// <summary>
        ///     Factory method which Generate board with random data according to parameters
        ///     horizontal WallsNumber will be spawned on random position on the cell, the same vertical number,
        ///     exit/Thesaus/Minotaur positions will be spawned randomly
        /// </summary>
        /// <param name="size">size of the grid</param>
        /// <param name="horizontalWallsNumber">number of horizontal walls</param>
        /// <param name="verticalWallsNumber">number of vertical walls</param>
        /// <returns></returns>
        public static BoardConfig GetRandom(Vector2Int size, int horizontalWallsNumber, int verticalWallsNumber)
        {
            var map = new Direction[size.y, size.x];

            var randomMap = ArrayUtils.GetOrderedSequence(size.x * size.y);
            randomMap.ShuffleArray();

            for (var i = 0; i < horizontalWallsNumber; i++)
            {
                var randomId = randomMap[i];
                var position = randomId.Get2DCoordinates(size.x);
                map[position.y, position.x] |= Direction.Left;
            }

            randomMap.ShuffleArray();
            for (var i = 0; i < verticalWallsNumber; i++)
            {
                var randomId = randomMap[i];
                var position = randomId.Get2DCoordinates(size.x);
                map[position.y, position.x] |= Direction.Up;
            }

            randomMap.ShuffleArray();

            BoardTextConverter.RawBoard.FillNeighbourWalls(map);

            return new BoardConfig(map,
                randomMap[0].Get2DCoordinates(size.x),
                randomMap[1].Get2DCoordinates(size.x),
                randomMap[2].Get2DCoordinates(size.x));
        }
    }
}