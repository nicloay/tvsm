using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using TheseusAndMinotaur.Data;
using TheseusAndMinotaur.Data.Deserializer;
using TheseusAndMinotaur.Data.Game;
using TheseusAndMinotaur.Data.Game.PathFinder;
using UnityEngine;

namespace TheseusAndMinotaur.Tests
{
    public class PathFindingTest
    {
        [Test, MaxTime(1000)]
        public void Test1()
        {
            var game = CreateGame("PathFinding1");
            var pathFinder = new PathFinder(game);
            var result = pathFinder.FindPath();
            Assert.That(result, Is.EqualTo(new []{Direction.Right, Direction.Up, Direction.Left}));
        }


        [Test]
        public void NodeHashTest()
        {
            var node1 = new Node(Vector2Int.left, Vector2Int.right, 12, Direction.Left);
            var node2 = new Node(Vector2Int.left, Vector2Int.right, 12, Direction.Left);
            var hashSet = new HashSet<Node>(Node.NodeComparer) { node1 };
            Assert.That(hashSet.Contains(node2), Is.True);
        }

        private static GameLogic CreateGame(string fileName)
        {
            return new GameLogic(
                BoardDeserializer.DeserializeFrom(File.ReadAllText($"UnitTestsData/{fileName}.txt")));
        }
    }
}