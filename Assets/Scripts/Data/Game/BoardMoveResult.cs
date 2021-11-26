using UnityEngine;

namespace TheseusAndMinotaur.Data.Game
{
    public class BoardMoveResult
    {
        /// <summary>
        ///     True - if board changed after user action
        ///     False - if Theseus and Minotaur stayed on the same positions
        /// </summary>
        public readonly bool BoardChanged;

        public readonly BoardStatus BoardStatus;
        public readonly Vector2Int MinotaurNewPosition;
        public readonly Direction[] Moves;
        public readonly Vector2Int TheseusNewPosition;


        /// <summary>
        ///     Result of the movement Theseus (1 turn) and Minotaur (2 turns)
        /// </summary>
        /// <param name="boardStatus">Status of the board after movement</param>
        /// <param name="moves">array of 3 elements [0] - theseus turn, [1:2] - minotaur turns</param>
        /// <param name="theseusNewPosition">Theseus position after movement</param>
        /// <param name="minotaurNewPosition">Minotaur position after movement</param>
        public BoardMoveResult(BoardStatus boardStatus, Direction[] moves, Vector2Int theseusNewPosition,
            Vector2Int minotaurNewPosition)
        {
            BoardStatus = boardStatus;
            Moves = moves;
            TheseusNewPosition = theseusNewPosition;
            MinotaurNewPosition = minotaurNewPosition;
            BoardChanged = moves[0] != Direction.None || moves[1] != Direction.None || moves[2] != Direction.None;
        }

        public Direction TheseusMove => Moves[0];
        public Direction MinotaurFirstMove => Moves[1];
        public Direction MinotaurSecondMove => Moves[2];
    }
}