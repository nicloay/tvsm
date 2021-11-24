using UnityEngine;
using UnityEngine.Assertions;

namespace TheseusAndMinotaur.Data
{
    /// <summary>
    ///     Data class which represent original board configuration.
    /// </summary>
    public class BoardConfig
    {
        internal readonly Direction[,] _wallMap;
        public readonly Vector2Int Exit;
        public readonly Vector2Int MinotaurStartPosition;
        public readonly Vector2Int TheseusStartPosition;
        
        
        public BoardConfig(Direction[,] wallMap, Vector2Int theseusStartPosition, Vector2Int minotaurStartPosition,
            Vector2Int exit)
        {
            Assert.IsTrue(theseusStartPosition != minotaurStartPosition,
                "theseus and minotaur position must be different");
            Assert.IsTrue(theseusStartPosition != exit, "theseus and exit position must be different");
            Assert.IsTrue(minotaurStartPosition != exit, "minotaur and exit position must be different");

            TheseusStartPosition = theseusStartPosition;
            MinotaurStartPosition = minotaurStartPosition;
            _wallMap = wallMap;
            Exit = exit;
        }

        /// <summary>
        ///     Get wall directions at specified position
        ///     pivot point: BottomLeft
        /// </summary>
        public Direction this[int y, int x] => _wallMap[y, x];

        /// <summary>
        ///     Get wall directions at specified position
        ///     pivot point: BottomLeft
        /// </summary>
        public Direction this[Vector2Int boardPosition] => this[boardPosition.y, boardPosition.x];

        public int Height => _wallMap.GetLength(0);
        public int Width => _wallMap.GetLength(1);

        public Vector2Int GridSize => new Vector2Int(Width, Height);
        
        public Direction[] GetWallsAtRow(int rowId)
        {
            var result = new Direction[Width];
            for (var x = 0; x < Width; x++) result[x] = _wallMap[rowId, x];
            return result;
        }
    }
}