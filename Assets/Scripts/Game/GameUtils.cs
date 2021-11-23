using TheseusAndMinotaur.Data;
using UnityEngine;

namespace TheseusAndMinotaur.Game
{
    public static class GameUtils
    {
        /// <summary>
        ///     Convert board position to world position
        /// </summary>
        public static Vector3 GetWorldPosition(this Vector2Int boardPosition)
        {
            var cellStep = GameConfig.Instance.CellStep;
            return new Vector3(boardPosition.x * cellStep.x, boardPosition.y * cellStep.y, 0);
        }

        /// <summary>
        ///     Return size in world coordinates
        /// </summary>
        public static Vector2 GetBoardWorldSize(this BoardConfig board)
        {
            var cellStep = GameConfig.Instance.CellStep;
            return new Vector2(cellStep.x * board.Width, cellStep.y * board.Height);
        }
    }
}