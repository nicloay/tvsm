using TheseusAndMinotaur.Data;
using UnityEngine;

namespace TheseusAndMinotaur.Maze
{
    /// <summary>
    /// Generate game object from board data
    /// place cells to proper positions.
    /// </summary>
    [RequireComponent(typeof(MazePools))]
    public class BoardGenerator : MonoBehaviour
    {
        [SerializeField] private Vector2 cellStep;
        
        private MazePools _mazePools;
        private void Awake()
        {
            _mazePools = GetComponent<MazePools>();
        }

        /// <summary>
        /// Generate walls for the board
        /// </summary>
        public void SpawnBoard(Board board)
        {
            Vector2 cellPosition = Vector2.zero;
            for (int y = 0; y < board.Height; y++)
            {
                cellPosition.x = 0;
                for (int x = 0; x < board.Width; x++)
                {
                    _mazePools.AddCell(cellPosition, board[y, x]);
                    cellPosition.x += cellStep.x;
                }

                cellPosition.y += cellStep.y;
            }
        }
        
        /// <summary>
        /// Clear Board (hide all cells)
        /// </summary>
        public void Clear()
        {
            _mazePools.Clear();
        }
    }
}