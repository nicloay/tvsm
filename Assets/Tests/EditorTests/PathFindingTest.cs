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
        [Test]
        public void Test1()
        {
            var game = CreateGame("PathFinding1");
            var pathFinder = new PathFinder(game);
            var (result, directions) = pathFinder.FindPath();
            Assert.That(result, Is.EqualTo(PathFinder.Result.SinglePathFound));
            Assert.That(directions, Is.EqualTo(new[] { Direction.Right, Direction.Up, Direction.Left }));
        }

        [Test]
        public void Test2()
        {
            var game = CreateGame("PathFinding2");
            var pathFinder = new PathFinder(game);
            var (result, directions) = pathFinder.FindPath();
            Assert.That(result, Is.EqualTo(PathFinder.Result.SinglePathFound));
            Assert.That(directions,
                Is.EqualTo(new[]
                {
                    Direction.Right,
                    Direction.Right,
                    Direction.Up,
                    Direction.Right,
                    Direction.Right,
                    Direction.Down,
                    Direction.Right,
                    Direction.Right,
                    Direction.Up,
                    Direction.Up,
                    Direction.Left,
                    Direction.Left,
                    Direction.Up,
                    Direction.Left,
                    Direction.Left,
                    Direction.Left,
                    Direction.Left,
                    Direction.Down,
                    Direction.Down,
                    Direction.Right
                }));
        }

        [Test]
        public void Estivalet1()
        {
            var game = CreateGame("estivalet1");
            var pathFinder = new PathFinder(game);
            var (result, directions) = pathFinder.FindPath();
            Debug.Log(string.Join(",", directions));
            Assert.That(result, Is.EqualTo(PathFinder.Result.SinglePathFound));
            Assert.That(directions,
                Is.EqualTo(new[]
                    { Direction.Left, Direction.Right, Direction.Right, Direction.Down, Direction.Right }));
        }

        [Test]
        public void Estivalet2()
        {
            var game = CreateGame("estivalet2");
            var pathFinder = new PathFinder(game);
            var (result, directions) = pathFinder.FindPath();
            Debug.Log(string.Join(",", directions));
            Assert.That(result, Is.EqualTo(PathFinder.Result.SinglePathFound));
            Assert.That(directions, Is.EqualTo(new[]
            {
                Direction.Right, Direction.Right, Direction.Right, Direction.Right,
                Direction.Up,
                Direction.Down, Direction.Down, Direction.Down,
                Direction.Left, Direction.Left, Direction.Left, Direction.Left, Direction.Left,
                Direction.Down
            }));
        }

        [Test]
        public void Estivalet3()
        {
            var game = CreateGame("estivalet3");
            var pathFinder = new PathFinder(game);
            var (result, directions) = pathFinder.FindPath();
            Debug.Log(string.Join(",", directions));
            Assert.That(result, Is.EqualTo(PathFinder.Result.MoreThanOneFound));
            Assert.That(directions, Is.EqualTo(new[]
            {
                Direction.Down, Direction.Down,
                Direction.Left,
                Direction.None,
                Direction.Right, Direction.Right,
                Direction.Up, Direction.Up,
                Direction.Right
            }));
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