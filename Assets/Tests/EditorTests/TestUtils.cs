using System.IO;
using NUnit.Framework;
using TheseusAndMinotaur.Data;
using TheseusAndMinotaur.Data.Deserializer;
using TheseusAndMinotaur.Data.Game;
using TheseusAndMinotaur.Data.Game.PathFinder;
using TheseusAndMinotaur.WorldControllers;
using UnityEngine;

namespace TheseusAndMinotaur.Tests
{
    public class TestUtils
    {
        [Test]
        public void CheckBoardToGlobalConversion()
        {
            var boardPosition = new Vector2Int(y: 5, x: 3);
            var globalPosition = boardPosition.GetWorldPosition();
            Assert.That(globalPosition, Is.EqualTo(new Vector3(y: 5f, x: 3f, z: 0f)));
        }

        /// <summary>
        ///     Create game from the file locate at PROJECT_HOME/UnitTestsData
        /// </summary>
        /// <param name="fileName">filename without extension</param>
        public static GameLogic CreateGame(string fileName)
        {
            var content = File.ReadAllText($"UnitTestsData/{fileName}.txt");
            return new GameLogic(content.ToBoardConfig());
        }

        /// <summary>
        ///     Create gameLogic from config, search for path, if exists, run
        /// </summary>
        /// <param name="config"></param>
        public static void RunBoardWithConfig(BoardConfig config)
        {
            var game = new GameLogic(config);
            RunGame(game);
        }

        private static void RunGame(GameLogic game)
        {
            var pathFinder = new PathFinder(game);
            var (exist, directions) = pathFinder.FindPath();
            if (exist != PathFinder.Result.PathNotFound)
            {
                foreach (var direction in directions)
                {
                    game.MakeMovement(direction);
                }
            }
            else
            {
                // Just random movement to try evaluate any movement if possible
                foreach (var direction in DirectionUtils.BaseDirections)
                {
                    if (game.IsMovementAvailable(game.TheseusCurrentPosition, direction))
                    {
                        game.MakeMovement(direction);
                    }
                }
            }
        }
    }
}