using System.Collections;
using TheseusAndMinotaur.Data;
using UnityEngine;
using UnityEngine.Assertions;

namespace TheseusAndMinotaur.Game
{
    /// <summary>
    ///     This controller is responsible in movement maze entities on the screen
    /// </summary>
    public class MovementController : MonoBehaviour
    {
        private BoardConfig _board;

        private Vector2Int _originalBoardPosition;
        public Vector2Int CurrentBoardPosition { get; private set; }

        public void Initialize(Vector2Int startPosition, BoardConfig board)
        {
            _originalBoardPosition = startPosition;
            _board = board;
            ResetToOriginalPosition();
        }

        public void ResetToOriginalPosition()
        {
            CurrentBoardPosition = _originalBoardPosition;
            transform.position = _originalBoardPosition.GetWorldPosition();
        }

        /// <summary>
        ///     Move entity to target board position and update internal state for board position
        ///     FutureTask: Should we use cancellation token and handle errors
        /// </summary>
        /// <param name="direction"></param>
        public IEnumerator MoveTo(Direction direction)
        {
            Assert.IsTrue(direction.IsBaseDirection());
            var currentPosition = transform.position;
            var targetBoardPoisition = CurrentBoardPosition.GetNeighbour(direction);
            var targetPosition = targetBoardPoisition.GetWorldPosition();
            var time = GameConfig.Instance.MovementSpeed;
            var currentTime = 0f;
            do
            {
                yield return null;
                currentTime += Time.unscaledDeltaTime;
                transform.position = Vector3.Lerp(currentPosition, targetPosition, currentTime / time);
            } while (currentTime < time);

            transform.position = targetPosition;
            CurrentBoardPosition = targetBoardPoisition;
        }

        public bool CanMoveTo(Direction direction)
        {
            var targetBoardPosition = CurrentBoardPosition.GetNeighbour(direction);
            return !_board[CurrentBoardPosition].HasWallAt(direction)
                   && targetBoardPosition.x >= 0
                   && targetBoardPosition.y >= 0
                   && targetBoardPosition.x <= _board.Width
                   && targetBoardPosition.y <= _board.Height;
        }
    }
}