using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheseusAndMinotaur.Data;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions;

namespace TheseusAndMinotaur.Game
{
    /// <summary>
    ///  This controller is responsible in movement maze entities on the screen 
    /// </summary>
    public class MovementController : MonoBehaviour
    {
        private Board _board;
        private Vector2Int _currentBoardPosition;

        public void Initialize(Vector2Int startPosition, Board board)
        {
            _board = board;
            _currentBoardPosition = startPosition;
            transform.position = startPosition.GetGlobalPosition();
        }

        /// <summary>
        /// Should we use cancellation token and handle errors here in future? 
        /// </summary>
        /// <param name="direction"></param>
        public async Task MoveTo(Direction direction)
        {
            Assert.IsTrue(direction.IsBaseDirection());
            var currentPosition = transform.position;
            var targetBoardPoisition = _currentBoardPosition.GetNeighbour(direction);
            var targetPosition = targetBoardPoisition.GetGlobalPosition();
            var time = GameConfig.Instance.MovementSpeed;
            var currentTime = 0f;
            do
            {
                await Task.Yield();
                currentTime += Time.unscaledDeltaTime;
                transform.position = Vector3.Lerp(currentPosition, targetPosition, currentTime / time);
            } while (currentTime < time);

            transform.position = targetPosition;
            _currentBoardPosition = targetBoardPoisition;
        }

        public bool CanMoveTo(Direction direction)
        {
            var targetBoardPosition = _currentBoardPosition.GetNeighbour(direction);
            return !_board[_currentBoardPosition].HasWallAt(direction)
                   && targetBoardPosition.x >= 0
                   && targetBoardPosition.y >= 0
                   && targetBoardPosition.x <= _board.Width
                   && targetBoardPosition.y <= _board.Height;
        }
    }
}