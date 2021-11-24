using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace TheseusAndMinotaur.Data
{
    /// <summary>
    ///     Base directions are:
    ///     * Left
    ///     * Right
    ///     * Top
    ///     * Down
    ///     Combined values are:
    ///     * All
    ///     * Horizontal
    ///     * Vertical
    /// </summary>
    [Flags]
    public enum Direction : byte
    {
        None = 0,
        Left = 1 << 0,
        Right = 1 << 1,
        Up = 1 << 2,
        Down = 1 << 3,

        All = Left | Right | Up | Down,
        Horizontal = Left | Right,
        Vertical = Up | Down
    }

    public static class DirectionUtils
    {
        /// <summary>
        ///     List of all possible directions
        /// </summary>
        public static readonly Direction[] BaseDirections =
            { Direction.Left, Direction.Right, Direction.Up, Direction.Down };

        public static readonly HashSet<Direction> BaseDirectionsHashSet = new(BaseDirections);


        private static readonly Dictionary<Direction, Vector2Int> OffsetByDirection = new()
        {
            { Direction.Left, new Vector2Int(-1, 0) },
            { Direction.Right, new Vector2Int(1, 0) },
            { Direction.Up, new Vector2Int(0, 1) },
            { Direction.Down, new Vector2Int(0, -1) }
        };


        private static readonly Dictionary<Direction, Direction> Opposites = new()
        {
            { Direction.Left, Direction.Right },
            { Direction.Right, Direction.Left },
            { Direction.Up, Direction.Down },
            { Direction.Down, Direction.Up },
            { Direction.None, Direction.None }
        };

        public static bool HasDirection(this Direction direction, Direction targetDirection)
        {
            if (((byte)direction & (byte)targetDirection) == (byte)targetDirection) return true;
            return false;
        }

        public static Direction GetOpposite(this Direction direction) => Opposites[direction];

        public static bool HasLeft(this Direction direction)
        {
            return direction.HasDirection(Direction.Left);
        }

        public static bool HasRight(this Direction direction)
        {
            return direction.HasDirection(Direction.Right);
        }

        public static bool HasTop(this Direction direction)
        {
            return direction.HasDirection(Direction.Up);
        }


        public static bool HasDown(this Direction direction)
        {
            return direction.HasDirection(Direction.Down);
        }

        /// <summary>
        /// Return true if there is wall in target direction
        /// </summary>
        public static bool HasWallAt(this Direction direction, Direction target)
        {
            return direction.HasDirection(target);
        }

        /// <summary>
        /// return true if the pass is clear and there is no walls in target direction
        /// </summary>
        public static bool HasWayTo(this Direction direction, Direction target) => !direction.HasWallAt(target);

        /// <summary>
        ///     Return list of base direction (Left, Right, Top, Down)
        ///     From the combined values
        /// </summary>
        public static IEnumerable<Direction> GetBaseDirections(this Direction direction)
        {
            return direction.Filter(BaseDirections);
        }

        /// <summary>
        ///     Indicate if direction is base (up, down, left, right) and not compound (e.g. Left+Top)
        /// </summary>
        public static bool IsBaseDirection(this Direction direction)
        {
            return BaseDirectionsHashSet.Contains(direction);
        }


        /// <summary>
        ///     Filter Directions with filter. (intersection)
        ///     eg.
        ///     Direction.All with filter (Left + Top) will return only (Left+ Top)
        ///     (Left+Bottom) with filter (Left + Top) will return (Left)
        /// </summary>
        public static IEnumerable<Direction> Filter(this Direction direction, Direction[] directions)
        {
            return directions.Where(targetDirection => direction.HasDirection(targetDirection));
        }

        /// <summary>
        ///     return neighbour cell in required direction
        /// </summary>
        /// <param name="boardPosition"></param>
        /// <param name="direction">must be base direction (up or left or down or top, not combination)</param>
        /// <returns></returns>
        public static Vector2Int GetNeighbour(this Vector2Int boardPosition, Direction direction)
        {
            if (direction == Direction.None)
            {
                return boardPosition;
            }

            Assert.IsTrue(direction.IsBaseDirection());
            return boardPosition + OffsetByDirection[direction];
        }
    }
}