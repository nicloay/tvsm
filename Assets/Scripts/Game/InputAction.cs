using System.Collections.Generic;
using TheseusAndMinotaur.Data;

namespace TheseusAndMinotaur.WorldControllers
{
    /// <summary>
    ///     AvailableInputAction
    /// </summary>
    public enum InputAction
    {
        None,
        MoveLeft,
        MoveRight,
        MoveUp,
        MoveDown,
        Undo,
        Restart,
        Wait,
        Next,
        Hint
    }

    public static class InputActionUtil
    {
        private static readonly Dictionary<InputAction, Direction> DirectionByInputAction = new()
        {
            { InputAction.MoveDown, Direction.Down },
            { InputAction.MoveLeft, Direction.Left },
            { InputAction.MoveRight, Direction.Right },
            { InputAction.MoveUp, Direction.Up },
            { InputAction.Wait, Direction.None }
        };

        /// <summary>
        ///     Convert input to proper direction
        /// </summary>
        public static Direction ToDirection(this InputAction inputAction)
        {
            return DirectionByInputAction[inputAction];
        }
    }
}