using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheseusAndMinotaur.Data.Game.PathFinder
{
    public class Node
    {
        public readonly Vector2Int TheseusPosition;
        public readonly Vector2Int MinotaurPosition;
        public readonly Node PreviousNode;
        public readonly int Cost;
        public readonly Direction DirectionFromPreviousNode;

        public Node(Vector2Int theseusPosition, Vector2Int minotaurPosition, int cost, Direction directionFromPreviousNode, Node previousNode = null)
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