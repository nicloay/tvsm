using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TheseusAndMinotaur.Data.Game.PathFinder
{
    /// <summary>
    ///     This pathfinder is based on A*, it search for the path of the Minotaur to the exit)
    ///     Minotaur position is taken from the GameLogic game
    /// </summary>
    public class PathFinder
    {
        private readonly GameLogic _gameLogic;

        public PathFinder(GameLogic gameLogic)
        {
            _gameLogic = gameLogic; 
        }

        /// <summary>
        /// </summary>
        /// <returns>
        ///     true or false - for the first parameter if path is found
        ///     List{Direction} - set of actions required to reach the target
        /// </returns>
        public (bool, List<Direction>) FindPath()
        {
            var theseusCurrentPosition = _gameLogic.TheseusCurrentPosition;
            var minotaurCurrentPosition = _gameLogic.MinotaurCurrentPosition;
            var currentNodes = new List<Node>
            {
                GetNode(theseusCurrentPosition, minotaurCurrentPosition)
            };

            var visitedNodes = new HashSet<Node>(Node.NodeComparer);

            while (currentNodes.Count > 0)
            {
                var node = currentNodes.OrderBy(node => node.TotalCost).First();

                currentNodes.Remove(node);
                visitedNodes.Add(node);

                foreach (var direction in DirectionUtils.MovementDirections)
                {
                    if (!_gameLogic.IsMovementAvailable(node.TheseusPosition, direction))
                    {
                        continue;
                    }

                    var result = _gameLogic.EvaluateMovement(node.TheseusPosition, node.MinotaurPosition, direction);
                    if (!result.BoardChanged)
                    {
                        continue;
                    }

                    if (result.BoardStatus == BoardStatus.GameOver)
                    {
                        continue;
                    }

                    if (result.BoardStatus == BoardStatus.Victory)
                    {
                        return (true, BuildPath(direction, node));
                    }

                    var newNode = GetNode(result.TheseusNewPosition, result.MinotaurNewPosition, node, direction);

                    if (visitedNodes.Contains(newNode))
                    {
                        continue;
                    }

                    currentNodes.Add(newNode);
                }
            }

            return (false, null);
        }

        private static List<Direction> BuildPath(Direction lastDirection, Node node)
        {
            var result = new List<Direction> { lastDirection };
            while (node.PreviousNode != null)
            {
                result.Add(node.Direction);
                node = node.PreviousNode;
            }

            result.Reverse();
            return result;
        }


        /// <summary>
        ///     return node which contains current minotaur and theseus position,
        ///     also calculate the cost for this position
        /// </summary>
        private Node GetNode(Vector2Int theseusPosition, Vector2Int minotaurPosition, Node previousNode = null,
            Direction direction = Direction.None)
        {
            return new Node(theseusPosition, minotaurPosition,
                GetReachExitCost(theseusPosition), direction, previousNode);
        }


        private int GetReachExitCost(Vector2Int theseusPosition)
        {
            // TODO: check maybe instead of path to start node, consider how many steps was made for this node.
            var theseus = _gameLogic.ExitPosition - theseusPosition;
            return Mathf.Abs(theseus.x) + Mathf.Abs(theseus.y);
        }
    }
}