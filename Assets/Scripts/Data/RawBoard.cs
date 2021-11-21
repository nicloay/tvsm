using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace TheseusAndMinotaur.Data
{
    /// <summary>
    ///  Internal class, used only to quickly update cells by index (without worrying if row or column exists in the target array)
    /// All internal maps and board arrays in the format [y, x]
    /// 0 row index is on the top at _directions array
    /// </summary>
    public class RawBoard
    {
        private readonly Dictionary<Vector2Int, Direction> _directions = new();
        private int _maxX; // max x of assigned values
        private int _maxY; // max y of assigned values

        public Direction this[Vector2Int position]
        {
            get => this[position.y, position.x];
            set => this[position.y, position.x] = value;
        }

        public Direction this[int y, int x]
        {
            get => _directions.TryGetValue(new Vector2Int(x, y), out var direction)
                ? direction
                : Direction.None;
            set
            {
                _maxX = math.max(_maxX, x);
                _maxY = math.max(_maxY, y);

                _directions[new Vector2Int(x, y)] = value;
            }
        }

        public void AddWall( int y, int x, Direction wallDirection)
        {
            this[ y,x] |= wallDirection;
        }

        public Board ConvertToBoard()
        {
            var map = new Direction[_maxY + 1, _maxX + 1];
            // 1. invert indexes (y on the bottom)
            for (int y = 0; y <= _maxY; y++)
            {
                for (int x = 0; x <= _maxX; x++)
                {
                    map[_maxY - y, x] = this[y, x];
                }
            }

            // 2. fill walls for every cells (so any cells will have info about left, right, top, down cells
            for (int y = 0, nextY = 1; y <= _maxY; y = nextY++)
            {
                for (int x = 0, nextX = 1; x <= _maxX; x = nextX++)
                {
                    if (nextX <= _maxX && map[y, nextX].HasLeft())
                    {
                        map[y, x] |= Direction.Right;
                    }

                    if (nextY <= _maxY && map[y, x].HasTop())
                    {
                        map[nextY, x] |= Direction.Down;
                    }
                }
            }

            return new Board(map, Vector2Int.zero, Vector2Int.zero,
                Vector2Int.zero); // FIXME: add proper vector values here
        }
    }
}