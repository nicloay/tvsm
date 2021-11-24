using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace TheseusAndMinotaur.Data.Game
{
    /// <summary>
    ///     This class implement game logic for the Theseus and Minotaur Game
    ///     TODO: rename Data namespace as it contains also logic
    /// </summary>
    public class GameLogic
    {
        private readonly BoardConfig _config;
        private readonly Stack<Direction> _history = new();
        public readonly Vector2Int ExitPosition;


        public GameLogic(BoardConfig config)
        {
            _config = config;
            Status = BoardStatus.Active;
            ExitPosition = config.Exit;
            Reset();
        }

        public Vector2Int MinotaurCurrentPosition { get; private set; }
        public Vector2Int TheseusCurrentPosition { get; private set; }


        public BoardStatus Status { get; private set; }

        public bool HasUndo => _history.Count > 0;

        public Vector2Int GridSize => _config.GridSize;

        /// <summary>
        ///     Reset Game to initial state
        ///     Theseus and Minotaur goes back to original positions
        /// </summary>
        public void Reset()
        {
            _history.Clear();
            Status = BoardStatus.Active;
            TheseusCurrentPosition = _config.TheseusStartPosition;
            MinotaurCurrentPosition = _config.MinotaurStartPosition;
        }

        /// <summary>
        ///     Move theseus to the direction
        ///     if direction = Direction.None, theseus skip the turn
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public BoardMoveResult MakeMovement(Direction direction)
        {
            var result = EvaluateMovement(TheseusCurrentPosition, MinotaurCurrentPosition, direction);
            RecordToHistoryUpdateStatus(result);
            return result;
        }

        /// <summary>
        ///     Check where Minotaur will go if Theseus make step in to the direction,
        ///     doesn't affect the board status
        ///     Exit position and maze layout is taken from current <see cref="BoardConfig" /> assigned to this logic instance
        /// </summary>
        /// <param name="theseusEvalPosition">theseus postion</param>
        /// <param name="minotaurEvalPosition">minotaur position</param>
        /// <param name="direction">direction which Theseus make</param>
        /// <returns>result of the board from this action</returns>
        public BoardMoveResult EvaluateMovement(Vector2Int theseusEvalPosition, Vector2Int minotaurEvalPosition,
            Direction direction)
        {
            Assert.IsTrue(IsMovementAvailable(theseusEvalPosition, direction),
                $"theseus are not able to make {direction} move from {theseusEvalPosition}");
            Direction[] moves = { direction, Direction.None, Direction.None };

            if (direction != Direction.None)
            {
                theseusEvalPosition = theseusEvalPosition.GetNeighbour(direction);
            }

            if (theseusEvalPosition == ExitPosition)
            {
                return new BoardMoveResult(BoardStatus.Victory, moves, theseusEvalPosition,
                    minotaurEvalPosition);
            }

            if (theseusEvalPosition == minotaurEvalPosition)
            {
                return new BoardMoveResult(BoardStatus.GameOver, moves, theseusEvalPosition,
                    minotaurEvalPosition);
            }

            for (var i = 0; i < 2; i++)
            {
                var minotaurDirection = GetMinotaurDirection(theseusEvalPosition, minotaurEvalPosition);
                moves[i + 1] = minotaurDirection;
                if (minotaurDirection == Direction.None)
                {
                    break; // no point to wait another turns
                }

                minotaurEvalPosition = minotaurEvalPosition.GetNeighbour(minotaurDirection);
                if (theseusEvalPosition == minotaurEvalPosition)
                {
                    return new BoardMoveResult(BoardStatus.GameOver, moves, theseusEvalPosition,
                        minotaurEvalPosition);
                }
            }

            return new BoardMoveResult(BoardStatus.Active, moves, theseusEvalPosition, minotaurEvalPosition);
        }


        public BoardMoveResult Undo()
        {
            Status = BoardStatus.Active;
            Assert.IsTrue(HasUndo);
            var mFirstMove = _history.Pop().GetOpposite();
            var mLastMove = _history.Pop().GetOpposite();
            var tMove = _history.Pop().GetOpposite();
            TheseusCurrentPosition = TheseusCurrentPosition.GetNeighbour(tMove);
            MinotaurCurrentPosition = MinotaurCurrentPosition.GetNeighbour(mFirstMove);
            MinotaurCurrentPosition = MinotaurCurrentPosition.GetNeighbour(mLastMove);
            return new BoardMoveResult(BoardStatus.Active, new[] { tMove, mFirstMove, mLastMove },
                TheseusCurrentPosition, MinotaurCurrentPosition);;
        }


        /// <summary>
        ///     Fixme: call this on actual movement
        /// </summary>
        private void RecordToHistoryUpdateStatus(BoardMoveResult movementResult)
        {
            Status = movementResult.BoardStatus;

            if (!movementResult.BoardChanged)
            {
                return;
            }

            TheseusCurrentPosition = TheseusCurrentPosition.GetNeighbour(movementResult.TheseusMove);
            MinotaurCurrentPosition = MinotaurCurrentPosition
                .GetNeighbour(movementResult.MinotaurFirstMove)
                .GetNeighbour(movementResult.MinotaurSecondMove);
            foreach (var direction in movementResult.Moves)
            {
                _history.Push(direction);
            }
        }

        private Direction GetMinotaurDirection(Vector2Int theseusEvalPosition, Vector2Int minotaurEvalPosition)
        {
            var diff = theseusEvalPosition - minotaurEvalPosition;

            var minotaurDirections = _config[minotaurEvalPosition];
            if (diff.x < 0 && minotaurDirections.HasWayTo(Direction.Left))
            {
                return Direction.Left;
            }

            if (diff.x > 0 && minotaurDirections.HasWayTo(Direction.Right))
            {
                return Direction.Right;
            }

            if (diff.y < 0 && minotaurDirections.HasWayTo(Direction.Down))
            {
                return Direction.Down;
            }

            if (diff.y > 0 && minotaurDirections.HasWayTo(Direction.Up))
            {
                return Direction.Up;
            }

            return Direction.None;
        }

        /// <summary>
        ///     Check if this move available for Theseus
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public bool IsMoveAvailableForTheseus(Direction direction)
        {
            return IsMovementAvailable(TheseusCurrentPosition, direction);
        }
        
        /// <summary>
        ///     Check if it's possible to step into direction from provided position
        ///     - no walls
        ///     - not on the edge
        /// </summary>
        public bool IsMovementAvailable(Vector2Int sourcePosition, Direction direction)
        {
            if (direction == Direction.None)
            {
                return true;
            }
            
            var targetBoardPosition = sourcePosition.GetNeighbour(direction);
            return _config[sourcePosition].HasWayTo(direction) 
                   && targetBoardPosition.x >= 0
                   && targetBoardPosition.y >= 0
                   && targetBoardPosition.x <= _config.Width
                   && targetBoardPosition.y <= _config.Height;
        }
    }
}