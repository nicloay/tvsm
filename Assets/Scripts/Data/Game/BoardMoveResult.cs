using UnityEngine;

namespace TheseusAndMinotaur.Data
{
    public class BoardMoveResult
    {
        public readonly BoardStatus BoardStatus;
        public readonly Direction[] Moves;
        public Direction TheseusMove => Moves[0];
        public Direction MinotaurFirstMove => Moves[1];
        public Direction MinotaurSecondMove => Moves[2];
        
        /// <summary>
        /// True - if board changed after user action
        /// False - if Theseus and Minotaur stayed on the same positions
        /// </summary>
        public readonly bool BoardChanged; 
        
        public BoardMoveResult(BoardStatus boardStatus, Direction[] moves, bool boardChanged)
        {
            BoardStatus = boardStatus;
            Moves = moves;
            this.BoardChanged = boardChanged;
        }
    }
}