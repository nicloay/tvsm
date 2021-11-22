using TheseusAndMinotaur.Data;
using UnityEngine;

namespace TheseusAndMinotaur.Game
{
    public static class GameUtils
    {
        public static Vector3 GetGlobalPosition(this Vector2Int boardPosition)
        {
            var cellStep = GameConfig.Instance.CellStep;
            return new Vector3(boardPosition.x * cellStep.x, boardPosition.y * cellStep.y, 0);
        }
    }
}