using TheseusAndMinotaur.Data;
using UnityEngine;

namespace TheseusAndMinotaur.Maze
{
    /// <summary>
    ///     Generate game object from board data
    ///     place cells to proper positions.
    /// </summary>
    [RequireComponent(typeof(MazePools))]
    public class BoardGenerator : MonoBehaviour
    {
        private MazePools _mazePools;

        private void Awake()
        {
            _mazePools = GetComponent<MazePools>();
        }

        /// <summary>
        ///     Generate walls for the board
        /// </summary>
        public void SpawnBoard(Board board)
        {
            var cellStep = GameConfig.Instance.CellStep;
            var cellPosition = Vector2.zero;
            for (var y = 0; y < board.Height; y++)
            {
                cellPosition.x = 0;
                for (var x = 0; x < board.Width; x++)
                {
                    _mazePools.AddCell(cellPosition, board[y, x]);
                    cellPosition.x += cellStep.x;
                }

                cellPosition.y += cellStep.y;
            }
        }

        /// <summary>
        ///     Clear Board (hide all cells)
        /// </summary>
        public void Clear()
        {
            _mazePools.Clear();
        }
    }
}