using UnityEngine;

namespace TheseusAndMinotaur.Data
{
    public class Board
    {
        public readonly Direction[,] Map;
        public readonly Vector2Int TheseusStartPosition;
        public readonly Vector2Int MinotaurStartPosition;
        public readonly Vector2Int Exit;

        public Board(Direction[,] map, Vector2Int theseusStartPosition, Vector2Int minotaurStartPosition, Vector2Int exit)
        {
            TheseusStartPosition = theseusStartPosition;
            MinotaurStartPosition = minotaurStartPosition;
            Map = map;
            Exit = exit;
        }
    }
}