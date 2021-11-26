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
    }
}