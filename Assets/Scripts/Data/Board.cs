using UnityEngine;

namespace TheseusAndMinotaur.Data
{
    /// <summary>
    ///     Data class which represent original board configuration.
    /// </summary>
    public class Board
    {
        private readonly Direction[,] _map;
        public readonly Vector2Int Exit;
        public readonly Vector2Int MinotaurStartPosition;
        public readonly Vector2Int TheseusStartPosition;

        public Board(Direction[,] map, Vector2Int theseusStartPosition, Vector2Int minotaurStartPosition,
            Vector2Int exit)
        {
            TheseusStartPosition = theseusStartPosition;
            MinotaurStartPosition = minotaurStartPosition;
            _map = map;
            Exit = exit;
        }

        /// <summary>
        ///     Get wall directions at specified position
        ///     pivot point: BottomLeft
        /// </summary>
        public Direction this[int y, int x] => _map[y, x];

        /// <summary>
        ///     Get wall directions at specified position
        ///     pivot point: BottomLeft
        /// </summary>
        public Direction this[Vector2Int boardPosition] => this[boardPosition.y, boardPosition.x];
        
        public int Height => _map.GetLength(0);
        public int Width => _map.GetLength(1);

        public Direction[] GetWallsAtRow(int rowId)
        {
            var result = new Direction[Width];
            for (var x = 0; x < Width; x++) result[x] = _map[rowId, x];
            return result;
        }
    }
}