using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace TheseusAndMinotaur.Data
{
    /// <summary>
    /// This class implement game logic for the Theseus and Minotaur Game
    /// </summary>
    public class BoardGame
    {
        private Stack<Direction> _history = new Stack<Direction>(); 
        
        private readonly BoardConfig _config;
        private Vector2Int _theseusCurrentPosition;
        private Vector2Int _minotaurCurrentPosition;
        private Vector2Int ExitPosition => _config.Exit;

        public BoardStatus Staus { get; private set; }

        public bool CanUndo() => _history.Count > 0;

        public BoardGame(BoardConfig config)
        {
            _config = config;
            Staus = BoardStatus.Active;
            Reset();
        }

        /// <summary>
        /// Reset Game to initial state
        /// Theseus and Minotaur goes back to original positions
        /// </summary>
        public void Reset()
        {
            _history.Clear();
            Staus = BoardStatus.Active;
            _theseusCurrentPosition = _config.TheseusStartPosition;
            _minotaurCurrentPosition = _config.MinotaurStartPosition;
        }

        private static readonly Vector2Int[] EmptyArray = new Vector2Int[0];

        /// <summary>
        /// Move theseus to the direction
        /// if direction = Direction.None, theseus skip the turn
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public BoardMoveResult MakeMovement(Direction direction)
        {
            Assert.IsTrue(IsMoveAvailableForTheseus(direction), $"theseus are not able to make {direction} move");
            Direction[] moves = new [] { direction, Direction.None, Direction.None };
            
            if (direction != Direction.None)
            {
                _theseusCurrentPosition = _theseusCurrentPosition.GetNeighbour(direction);
            }

            if (_theseusCurrentPosition == ExitPosition)
            {
                return GetResult(BoardStatus.Victory, moves);
            }

            if (_theseusCurrentPosition == _minotaurCurrentPosition)
            {
                return GetResult(BoardStatus.GameOver, moves);
            }

            for (int i = 0; i < 2; i++)
            {
                var minotaurDirection = GetMinotaurDirection();
                moves[i + 1] = minotaurDirection;
                if (minotaurDirection == Direction.None)
                {
                    break; // no point to wait another turns
                }

                _minotaurCurrentPosition = _minotaurCurrentPosition.GetNeighbour(minotaurDirection);
                if (_theseusCurrentPosition == _minotaurCurrentPosition)
                {
                    return GetResult(BoardStatus.GameOver, moves);
                }
            }

            return GetResult(BoardStatus.Active, moves);
        }
        
        public BoardMoveResult Undo()
        {
            Staus = BoardStatus.Active;
            Assert.IsTrue(CanUndo());
            var mLastMove = _history.Pop();
            var mFirstMove = _history.Pop();
            var tMove = _history.Pop();
            return new BoardMoveResult(BoardStatus.Active, new[] { tMove, mFirstMove, mLastMove });
        }

        private BoardMoveResult GetResult(BoardStatus status, Direction[] moves)
        {
            Staus = status;
            for (int i = 0; i < moves.Length; i++)
            {
                _history.Push(moves[i]);
            }
            return new BoardMoveResult(status, moves);
        }

        private Direction GetMinotaurDirection()
        {
            var diff =   _theseusCurrentPosition - _minotaurCurrentPosition;

            var minotaurDirections = _config[_minotaurCurrentPosition];
            if (diff.x < 0 && minotaurDirections.HasWayTo(Direction.Left)) return Direction.Left;

            if (diff.x > 0 && minotaurDirections.HasWayTo(Direction.Right)) return Direction.Right;

            if (diff.y < 0 && minotaurDirections.HasWayTo(Direction.Down)) return Direction.Down;

            if (diff.y > 0 && minotaurDirections.HasWayTo(Direction.Up)) return Direction.Up;

            return Direction.None;
        }
        
        
        /// <summary>
        /// Check if this move available for Theseus
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public bool IsMoveAvailableForTheseus(Direction direction)
        {
            if (direction == Direction.None)
            {
                return true;
            }

            var targetBoardPosition = _theseusCurrentPosition.GetNeighbour(direction);
            return _config[_theseusCurrentPosition].HasWayTo(direction)
                   && targetBoardPosition.x >= 0
                   && targetBoardPosition.y >= 0
                   && targetBoardPosition.x <= _config.Width
                   && targetBoardPosition.y <= _config.Height;
        }
    }
}