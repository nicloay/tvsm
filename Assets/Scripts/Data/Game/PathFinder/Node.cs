using System;
using UnityEngine;

namespace TheseusAndMinotaur.Data.Game.PathFinder
{
    public class Node
    {
        public readonly int Cost;
        public readonly Direction DirectionFromPreviousNode;
        public readonly Vector2Int MinotaurPosition;
        public readonly Node PreviousNode;
        public readonly Vector2Int TheseusPosition;

        public Node(Vector2Int theseusPosition, Vector2Int minotaurPosition, int cost,
            Direction directionFromPreviousNode, Node previousNode = null)
        {
            MinotaurPosition = minotaurPosition;
            DirectionFromPreviousNode = directionFromPreviousNode;
            TheseusPosition = theseusPosition;
            PreviousNode = previousNode;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(TheseusPosition, MinotaurPosition);
        }
    }
}