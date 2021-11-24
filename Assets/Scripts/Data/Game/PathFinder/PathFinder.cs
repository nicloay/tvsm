using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TheseusAndMinotaur.Data.Game.PathFinder
{
    /// <summary>
    ///     This pathfinder is based on A*, it search for the path of the Minotaur to the exit)
    ///     Minotaur position is taken from the GameLogic game
    ///     it use normal methods to move minotaur to evaluate new positions on this movement and then undo this actions.
    /// </summary>
    public class PathFinder
    {
        private readonly GameLogic _gameLogic;
        private Vector2Int _theseusStartPosition;

        public PathFinder(GameLogic gameLogic)
        {
            _gameLogic = gameLogic;
        }


        /// <summary>
        /// </summary>
        /// <returns>
        ///     true or false - for the first parameter if path is found
        ///     List<direction> - set of actions required to reach the target
        /// </returns>
        public (bool, List<Direction>) FindPath()
        {
            _theseusStartPosition = _gameLogic.TheseusCurrentPosition;
            var currentNodes = new List<Node>
            {
                GetNode()
            };

            var visitedNodes = new HashSet<Node>();

            while (currentNodes.Count > 0)
            {
                var node = currentNodes.OrderBy(node => node.Cost).First();
                currentNodes.Remove(node);
                visitedNodes.Add(node);
                if (node.MinotaurPosition == _gameLogic.ExitPosition) return (true, BuildPath(node));

                foreach (var direction in DirectionUtils.MovementDirections)
                {
                    if (!_gameLogic.IsMoveAvailableForTheseus(direction)) continue;

                    var result = _gameLogic.MakeMovement(direction);
                    _gameLogic.Undo();
                    if (!result.BoardChanged) continue;

                    if (result.BoardStatus == BoardStatus.GameOver) continue;

                    var newNode = GetNode(node, direction);
                    if (visitedNodes.Contains(newNode)) continue;
                    // we can check also win condition here, but anyway it will be triggered on next iteration, so let's save some lines 
                    currentNodes.Add(newNode);
                }
            }

            return (false, null);
        }

        private List<Direction> BuildPath(Node node)
        {
            var result = new List<Direction>();
            while (node.PreviousNode != null)
            {
                result.Add(node.DirectionFromPreviousNode);
                node = node.PreviousNode;
            }

            result.Reverse();
            return result;
        }


        /// <summary>
        ///     return node which contains current minotaur and theseus position,
        ///     also calculate the cost for this position
        /// </summary>
        /// <returns></returns>
        private Node GetNode(Node previousNode = null, Direction directionFromPreviousNode = Direction.None)
        {
            return new Node(_gameLogic.TheseusCurrentPosition, _gameLogic.MinotaurCurrentPosition,
                GetCurrentPositionsCost(), directionFromPreviousNode, previousNode);
        }


        private int GetCurrentPositionsCost()
        {
            // TODO: check maybe instead of path to start node, consider how many steps was made for this node.
            var theseus = _gameLogic.ExitPosition - _gameLogic.TheseusCurrentPosition;
            var minotaur = _theseusStartPosition - _gameLogic.TheseusCurrentPosition;
            return Mathf.Abs(theseus.x) + Mathf.Abs(theseus.y) + Mathf.Abs(minotaur.x) + Mathf.Abs(minotaur.y);
        }
    }
}