using TheseusAndMinotaur.Data;
using TheseusAndMinotaur.WorldControllers.Maze;
using UnityEngine;

namespace TheseusAndMinotaur.Visual.World
{
    /// <summary>
    ///     Generate game object from board data
    ///     place cells to proper positions.
    /// </summary>
    [RequireComponent(typeof(GridPools))]
    public class WorldGridManager : MonoBehaviour
    {
        private GridPools _gridPools;

        private void Awake()
        {
            _gridPools = GetComponent<GridPools>();
        }

        /// <summary>
        ///     Generate walls for the board
        /// </summary>
        public void SpawnBoard(BoardConfig boardConfig)
        {
            Clear();
            var cellStep = GameConfig.Instance.CellStep;
            var cellPosition = Vector2.zero;
            for (var y = 0; y < boardConfig.Height; y++)
            {
                cellPosition.x = 0;
                for (var x = 0; x < boardConfig.Width; x++)
                {
                    _gridPools.AddCell(cellPosition, boardConfig[y, x]);
                    cellPosition.x += cellStep.x;
                }

                cellPosition.y += cellStep.y;
            }
        }

        /// <summary>
        ///     Clear Board (hide all cells)
        /// </summary>
        private void Clear()
        {
            _gridPools.Clear();
        }
    }
}