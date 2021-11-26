using System.IO;
using TheseusAndMinotaur.Data;
using TheseusAndMinotaur.Data.Converter;
using TheseusAndMinotaur.Data.Game;
using TheseusAndMinotaur.Data.Game.PathFinder;

namespace TheseusAndMinotaur.Tests.Tests.EditorTests
{
    /// <summary>
    ///     util methods used by another tests
    /// </summary>
    public class TestUtils
    {
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