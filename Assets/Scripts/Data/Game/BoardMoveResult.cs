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
        
        
        public BoardMoveResult(BoardStatus boardStatus, Direction[] moves)
        {
            BoardStatus = boardStatus;
            Moves = moves;
        }
    }
}