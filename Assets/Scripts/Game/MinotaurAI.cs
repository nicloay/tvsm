using TheseusAndMinotaur.Data;
using UnityEngine;

namespace TheseusAndMinotaur.Game
{
    [RequireComponent(typeof(MovementController))]
    public class MinotaurAI : MonoBehaviour
    {
        private Board _board;
        private MovementController _target;
        private MovementController _selfMovementController;
        
        public void Initialize(MovementController target, Board board)
        {
            _selfMovementController = GetComponent<MovementController>();
            _selfMovementController.Initialize(board.MinotaurStartPosition, board);
            _target = target;
            _board = board;
        }


        public Direction GetDirectionToTheTarget()
        {
            var diff = _target.CurrentBoardPosition - _selfMovementController.CurrentBoardPosition;
            if (diff.x < 0 && _selfMovementController.CanMoveTo(Direction.Left))
            {
                return Direction.Left;
            }

            if (diff.x > 0 && _selfMovementController.CanMoveTo(Direction.Right))
            {
                return Direction.Right;
            }

            if (diff.y < 0 && _selfMovementController.CanMoveTo(Direction.Down))
            {
                return Direction.Down;
            }

            if (diff.y > 0 && _selfMovementController.CanMoveTo(Direction.Up))
            {
                return Direction.Up;
            }

            return Direction.None;
        }
    }
}