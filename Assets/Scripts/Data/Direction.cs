
using System;

namespace TheseusAndMinotaur.Data
{
    [Flags]
    public enum Direction : byte
    {
        None = 0,
        Left = 1 << 0,
        Right = 1 << 1,
        Top = 1 << 2,
        Down = 1 << 3,
      
        All = Left | Right | Top | Down,
        Horizontal = Left | Right,
        Vertical = Top | Down
    }

    public static class DirectionUtils
    {
        public static bool HasDirection(this Direction direction, Direction targetDirection)
        {
            if (((byte)direction & (byte) targetDirection) == (byte) targetDirection)
            {
                return true;
            }
            return false;
        }

        public static bool HasLeft(this Direction direction) => direction.HasDirection(Direction.Left);
        public static bool HasRight(this Direction direction) => direction.HasDirection(Direction.Right);
        public static bool HasTop(this Direction direction) => direction.HasDirection(Direction.Top);
        public static bool HasDown(this Direction direction) => direction.HasDirection(Direction.Down);
    }
}