using System.Collections.Generic;
using TheseusAndMinotaur.Data;

namespace TheseusAndMinotaur.Game
{
    /// <summary>
    ///     AvailableInputAction
    /// </summary>
    public enum InputAction
    {
        MoveLeft,
        MoveRight,
        MoveUp,
        MoveDown,
        Undo,
        Restart
    }

    public static class InputActionUtil
    {
        private static readonly Dictionary<InputAction, Direction> _directionByInputAction = new()
        {
            { InputAction.MoveDown, Direction.Down },
            { InputAction.MoveLeft, Direction.Left },
            { InputAction.MoveRight, Direction.Right },
            { InputAction.MoveUp, Direction.Up }
        };

        /// <summary>
        ///     Convert input to proper direction
        /// </summary>
        public static Direction ToDirection(this InputAction inputAction)
        {
            return _directionByInputAction[inputAction];
        }
    }
}