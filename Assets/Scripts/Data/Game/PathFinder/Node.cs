using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheseusAndMinotaur.Data.Game.PathFinder
{
    public class Node
    {
        private readonly int _costFromStart;
        public readonly Direction Direction;
        public readonly Vector2Int MinotaurPosition;
        public readonly Node PreviousNode;
        public readonly Vector2Int TheseusPosition;
        public readonly int TotalCost;

        public Node(Vector2Int theseusPosition, Vector2Int minotaurPosition, int costToReachExit,
            Direction direction, Node previousNode = null)
        {
            MinotaurPosition = minotaurPosition;
            Direction = direction;
            TheseusPosition = theseusPosition;
            PreviousNode = previousNode;
            _costFromStart = 1 + (previousNode?._costFromStart ?? 0);
            TotalCost = costToReachExit + _costFromStart;
        }

        public static IEqualityComparer<Node> NodeComparer { get; } = new NodeEqualityComparer();

        private sealed class NodeEqualityComparer : IEqualityComparer<Node>
        {
            public bool Equals(Node x, Node y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                if (ReferenceEquals(x, null))
                {
                    return false;
                }

                if (ReferenceEquals(y, null))
                {
                    return false;
                }

                if (x.GetType() != y.GetType())
                {
                    return false;
                }

                return x.MinotaurPosition.Equals(y.MinotaurPosition) && x.TheseusPosition.Equals(y.TheseusPosition);
            }

            public int GetHashCode(Node obj)
            {
                return HashCode.Combine(obj.MinotaurPosition, obj.TheseusPosition);
            }
        }
    }
}