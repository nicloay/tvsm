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
            Assert.IsTrue(IsMoveAvailableForTheseus(direction), $"theseus are not able to make {direction} move");
            Direction[] moves = { direction, Direction.None, Direction.None };

            if (direction != Direction.None) TheseusCurrentPosition = TheseusCurrentPosition.GetNeighbour(direction);

            if (TheseusCurrentPosition == ExitPosition) return GetResult(BoardStatus.Victory, moves);

            if (TheseusCurrentPosition == MinotaurCurrentPosition) return GetResult(BoardStatus.GameOver, moves);

            for (var i = 0; i < 2; i++)
            {
                var minotaurDirection = GetMinotaurDirection();
                moves[i + 1] = minotaurDirection;
                if (minotaurDirection == Direction.None) break; // no point to wait another turns

                MinotaurCurrentPosition = MinotaurCurrentPosition.GetNeighbour(minotaurDirection);
                if (TheseusCurrentPosition == MinotaurCurrentPosition) return GetResult(BoardStatus.GameOver, moves);
            }

            return GetResult(BoardStatus.Active, moves);
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
            return new BoardMoveResult(BoardStatus.Active, new[] { tMove, mFirstMove, mLastMove }, true);
        }

        private BoardMoveResult GetResult(BoardStatus status, Direction[] moves)
        {
            Status = status;
            var changed = moves[0] != Direction.None || moves[1] != Direction.None || moves[2] != Direction.None;
            if (changed)
                for (var i = 0; i < moves.Length; i++)
                    _history.Push(moves[i]);
            return new BoardMoveResult(status, moves, changed);
        }

        private Direction GetMinotaurDirection()
        {
            var diff = TheseusCurrentPosition - MinotaurCurrentPosition;

            var minotaurDirections = _config[MinotaurCurrentPosition];
            if (diff.x < 0 && minotaurDirections.HasWayTo(Direction.Left)) return Direction.Left;

            if (diff.x > 0 && minotaurDirections.HasWayTo(Direction.Right)) return Direction.Right;

            if (diff.y < 0 && minotaurDirections.HasWayTo(Direction.Down)) return Direction.Down;

            if (diff.y > 0 && minotaurDirections.HasWayTo(Direction.Up)) return Direction.Up;

            return Direction.None;
        }


        /// <summary>
        ///     Check if this move available for Theseus
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public bool IsMoveAvailableForTheseus(Direction direction)
        {
            if (direction == Direction.None) return true;

            var targetBoardPosition = TheseusCurrentPosition.GetNeighbour(direction);
            return _config[TheseusCurrentPosition].HasWayTo(direction)
                   && targetBoardPosition.x >= 0
                   && targetBoardPosition.y >= 0
                   && targetBoardPosition.x <= _config.Width
                   && targetBoardPosition.y <= _config.Height;
        }
    }
}