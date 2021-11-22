using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

// to test RawBoard in the unit tests we need following attribute
[assembly: InternalsVisibleTo("TheseusAndMinotaur.Tests")]

namespace TheseusAndMinotaur.Data.Deserializer
{
    public static partial class BoardDeserializer
    {
        /// <summary>
        ///     Internal class, used only to quickly update cells by index (without worrying if row or column exists in the target
        ///     array)
        ///     All internal maps and board arrays in the format [y, x]
        ///     PivotPoint: topLeft
        /// </summary>
        internal sealed class RawBoard
        {
            private readonly Dictionary<Vector2Int, Direction> _directions = new();
            public readonly SingleTimeSetValue<Vector2Int> ExitPosition = new();
            public readonly SingleTimeSetValue<Vector2Int> MinotaurStartPosition = new();
            public readonly SingleTimeSetValue<Vector2Int> TheseusStartPosition = new();
            private int _maxX = int.MinValue; // max x of assigned values
            private int _maxY = int.MinValue; // max y of assigned values


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

            public void AddWall(int y, int x, Direction wallDirection)
            {
                this[y, x] |= wallDirection;
            }

            /// <summary>
            ///     Convert data to the board
            ///     Board is an array with pivotPoint: bottomLeft
            /// </summary>
            /// <returns></returns>
            /// <exception cref="ParseMazeException"></exception>
            public Board ConvertToBoard()
            {
                if (_maxX < 0 && _maxY < 0) throw new ParseMazeException("maze contains no walls");

                if (!TheseusStartPosition.IsValueSet) throw new ParseMazeException("Theseus position is not set");

                if (!MinotaurStartPosition.IsValueSet) throw new ParseMazeException("Minotaur position is not set");

                if (!ExitPosition.IsValueSet) throw new ParseMazeException("Exit position is not set");

                var map = new Direction[_maxY + 1, _maxX + 1];
                // 1. invert Y indexes (move Y to the bottom)
                for (var y = 0; y <= _maxY; y++)
                for (var x = 0; x <= _maxX; x++)
                    map[_maxY - y, x] = this[y, x];

                // 2. fill walls for neighbour cells (so any cells will have info about left, right, top, down cells
                for (int y = 0, nextY = 1; y <= _maxY; y = nextY++)
                for (int x = 0, nextX = 1; x <= _maxX; x = nextX++)
                {
                    if (nextX <= _maxX && map[y, nextX].HasLeft()) map[y, x] |= Direction.Right;

                    if (nextY <= _maxY && map[y, x].HasTop()) map[nextY, x] |= Direction.Down;
                }

                return new Board(map,
                    ConvertToBoardPosition(TheseusStartPosition.Value),
                    ConvertToBoardPosition(MinotaurStartPosition.Value),
                    ConvertToBoardPosition(ExitPosition.Value));
            }

            /// <summary>
            ///     Original value is based on topLeft pivot point, result board pivot point is bottomLeft
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            private Vector2Int ConvertToBoardPosition(Vector2Int value)
            {
                return new Vector2Int(value.x, _maxY - value.y);
            }
        }
    }
}