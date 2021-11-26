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
        public void TestNoPath()
        {
            // 5_4 must fail, as pathfinding must not allow to step to the same position where minotaur stays at the moment
            var game = TestUtils.CreateGame("g5_4");
            var pathFinder = new PathFinder(game);
            var (result, directions) = pathFinder.FindPath();
            Assert.That(result, Is.EqualTo(PathFinder.Result.PathNotFound));
        }
        
        
        [Test]
        public void Test1()
        
        {
            var game = TestUtils.CreateGame("PathFinding1");
            var pathFinder = new PathFinder(game);
            var (result, directions) = pathFinder.FindPath();
            Assert.That(result, Is.EqualTo(PathFinder.Result.SinglePathFound));
            Assert.That(directions, Is.EqualTo(new[] { Direction.Right, Direction.Up, Direction.Left }));
        }

        [Test]
        public void Test2()
        {
            var game = TestUtils.CreateGame("PathFinding2");
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
            var game = TestUtils.CreateGame("estivalet1");
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
            var game = TestUtils.CreateGame("estivalet2");
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
            var game = TestUtils.CreateGame("estivalet3");
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
    }
}